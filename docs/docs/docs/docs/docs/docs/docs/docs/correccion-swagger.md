## Problema detectado
Durante las pruebas del endpoint de disponibilidad,
Swagger mostraba el siguiente error:

Please correct the following validation errors and try again.
For 'doctorId': Required field is not provided.

El mensaje aparecía incluso cuando se ingresaba
el identificador del médico.

## Endpoint afectado
GET /api/Schedules/availability

## Cambios realizados
Se ajustó la definición de los parámetros de consulta
en SchedulesController.

La versión probada utiliza:

[FromQuery] int doctorId
[FromQuery] DateOnly date

sin el atributo BindRequired.

También se corrigieron errores de compilación
introducidos durante la modificación del controlador.

## Validación posterior
Se compiló MediFlow correctamente mediante dotnet build.

Se ejecutó el endpoint desde Swagger con:
- doctorId: 1
- date: 2026-11-30

## Resultado
HTTP 200 OK

La respuesta contenía ocho turnos disponibles.

## Conclusión
La versión corregida permitió ejecutar la consulta
desde Swagger UI sin el error de validación observado.



Mensaje del commit:
Adhelmar: documentar correccion de Swagger
