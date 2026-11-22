-- Aplicar una vez al esquema existente. Si hay duplicados activos, PostgreSQL
-- rechaza el índice y conserva las asignaciones para su revisión.
CREATE UNIQUE INDEX IF NOT EXISTS "UX_DoctorRooms_ActiveDoctorRoom"
    ON public."DoctorRooms" ("DoctorId", "RoomId")
    WHERE "IsActive";
