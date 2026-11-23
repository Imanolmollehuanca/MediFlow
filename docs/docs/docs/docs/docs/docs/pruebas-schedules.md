## Objetivo
Verificar la administración de horarios médicos.

## Endpoints verificados
- GET /api/Schedules
- GET /api/Schedules/{id}
- POST /api/Schedules
- PUT /api/Schedules/{id}
- DELETE /api/Schedules/{id}

## Datos de prueba
- doctorId: 1
- scheduleId creado: 2
- Día de la semana: martes
- Horario inicial: 14:00 - 17:00 UTC
- Duración de turno: 30 minutos

## Procedimiento
1. Consultar los horarios existentes.
2. Crear un horario para el martes.
3. Consultar el horario creado mediante su ID.
4. Actualizar el horario a 15:00 - 18:00 UTC.
5. Consultar nuevamente para verificar el cambio.
6. Eliminar el horario de prueba.

## Resultados
- Creación: correcta.
- Consulta por ID: correcta.
- Actualización: correcta.
- Eliminación: correcta.

## Observación
El horario original con scheduleId 1 no fue eliminado.

## Conclusión
Se verificaron las operaciones CRUD del módulo Schedules.



Mensaje del commit:
Adhelmar: documentar pruebas CRUD de Schedules
