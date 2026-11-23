## Objetivo
Verificar la generación de turnos disponibles para
un médico según su horario registrado.

## Endpoint
GET /api/Schedules/availability

## Parámetros utilizados
- doctorId: 1
- date: 2026-11-30

## Solicitud
GET /api/Schedules/availability?doctorId=1&date=2026-11-30

## Resultado observado
- Código HTTP: 200 OK
- Cantidad de turnos: 8
- Duración de cada turno: 30 minutos
- Horario disponible: 08:00 - 12:00 UTC
- scheduleId: 1

## Turnos devueltos
1. 08:00 - 08:30
2. 08:30 - 09:00
3. 09:00 - 09:30
4. 09:30 - 10:00
5. 10:00 - 10:30
6. 10:30 - 11:00
7. 11:00 - 11:30
8. 11:30 - 12:00

## Observaciones
Todos los turnos devolvieron roomIds vacío porque
el médico no tenía una asignación activa de consultorio.

## Conclusión
La API generó correctamente los ocho turnos disponibles.



Mensaje del commit:
Adhelmar: documentar pruebas de disponibilidad medicaCommit 12 — Pruebas de disponibilidad
Nombre del archivo: docs/pruebas-disponibilidad.md

# Pruebas de disponibilidad médica - MediFlow

## Objetivo
Verificar la generación de turnos disponibles para
un médico según su horario registrado.

## Endpoint
GET /api/Schedules/availability

## Parámetros utilizados
- doctorId: 1
- date: 2026-11-30

## Solicitud
GET /api/Schedules/availability?doctorId=1&date=2026-11-30

## Resultado observado
- Código HTTP: 200 OK
- Cantidad de turnos: 8
- Duración de cada turno: 30 minutos
- Horario disponible: 08:00 - 12:00 UTC
- scheduleId: 1

## Turnos devueltos
1. 08:00 - 08:30
2. 08:30 - 09:00
3. 09:00 - 09:30
4. 09:30 - 10:00
5. 10:00 - 10:30
6. 10:30 - 11:00
7. 11:00 - 11:30
8. 11:30 - 12:00

## Observaciones
Todos los turnos devolvieron roomIds vacío porque
el médico no tenía una asignación activa de consultorio.

## Conclusión
La API generó correctamente los ocho turnos disponibles.



Mensaje del commit:
Adhelmar: documentar pruebas de disponibilidad medica
