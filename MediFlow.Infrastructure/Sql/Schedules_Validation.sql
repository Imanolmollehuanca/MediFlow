-- Aplicar en la base de trabajo para habilitar la protección ante escrituras simultáneas.
-- Si existen horarios activos inválidos o solapados, la operación falla y conserva
-- los registros para corregirlos antes de repetir el script.
BEGIN;

SET LOCAL search_path TO pg_catalog, public;
CREATE EXTENSION IF NOT EXISTS btree_gist WITH SCHEMA public;

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conrelid = 'public."Schedules"'::regclass
                   AND conname = 'CK_Schedules_ActiveDay') THEN
        ALTER TABLE public."Schedules" ADD CONSTRAINT "CK_Schedules_ActiveDay"
            CHECK (NOT "IsActive" OR "DayOfWeek" BETWEEN 0 AND 6);
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conrelid = 'public."Schedules"'::regclass
                   AND conname = 'CK_Schedules_ActiveTimes') THEN
        ALTER TABLE public."Schedules" ADD CONSTRAINT "CK_Schedules_ActiveTimes"
            CHECK (NOT "IsActive" OR "StartTime" < "EndTime");
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conrelid = 'public."Schedules"'::regclass
                   AND conname = 'CK_Schedules_ActiveDuration') THEN
        ALTER TABLE public."Schedules" ADD CONSTRAINT "CK_Schedules_ActiveDuration"
            CHECK (NOT "IsActive" OR ("SlotDurationMinutes" > 0 AND
                "SlotDurationMinutes" * INTERVAL '1 minute' <= "EndTime" - "StartTime"));
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conrelid = 'public."Schedules"'::regclass
                   AND conname = 'EX_Schedules_DoctorDayNoOverlap') THEN
        -- La fecha fija permite representar las horas como un rango sin zona horaria.
        -- [) incluye el inicio y excluye el fin, por lo que permite bloques consecutivos.
        ALTER TABLE public."Schedules" ADD CONSTRAINT "EX_Schedules_DoctorDayNoOverlap"
            EXCLUDE USING gist (
                "DoctorId" WITH =,
                "DayOfWeek" WITH =,
                (tsrange(DATE '2000-01-01' + "StartTime",
                         DATE '2000-01-01' + "EndTime", '[)')) WITH &&
            ) WHERE ("IsActive");
    END IF;
END $$;

COMMIT;
