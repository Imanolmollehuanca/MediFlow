## Objetivo
Documentar la verificación manual de los endpoints
del módulo de consultorios mediante Swagger UI.

## Endpoints verificados

- GET /api/Rooms
- GET /api/Rooms/{id}
- POST /api/Rooms
- PUT /api/Rooms/{id}
- DELETE /api/Rooms/{id}

## Resultados de las pruebas

| Operación | Resultado |
|-----------|-----------|
| Listar consultorios | Correcto |
| Consultar por ID | Correcto |
| Crear consultorio | Correcto |
| Actualizar consultorio | Correcto |
| Eliminación lógica | Correcto |

## Datos utilizados

- Consultorio activo: CONS-103
- roomId: 2
- Se verificó la eliminación lógica de roomId 1.

## Conclusión

Las operaciones CRUD del módulo Rooms fueron
verificadas mediante pruebas manuales en Swagger UI.
