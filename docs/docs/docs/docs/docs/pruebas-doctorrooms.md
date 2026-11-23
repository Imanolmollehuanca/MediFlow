## Objetivo
Documentar la gestión de asignaciones entre médicos
y consultorios mediante Swagger UI.

## Endpoints verificados
- GET /api/DoctorRooms
- GET /api/DoctorRooms/{id}
- POST /api/DoctorRooms
- PUT /api/DoctorRooms/{id}
- DELETE /api/DoctorRooms/{id}

## Datos de prueba
- doctorId: 1
- roomId: 2
- doctorRoomId generado: 1

## Resultados
1. Se consultaron las asignaciones existentes.
2. Se creó una asignación entre médico y consultorio.
3. Se consultó la asignación por su identificador.
4. Se actualizó la asignación a estado inactivo.
5. Se verificó la actualización.
6. Se ejecutó la eliminación lógica.

## Respuestas observadas
- Consulta por ID: HTTP 200.
- Actualización: HTTP 204.
- Eliminación: HTTP 204.

## Conclusión
Se verificó el funcionamiento del CRUD de DoctorRooms.



Mensaje del commit:
Adhelmar: documentar pruebas CRUD de DoctorRooms
