## Objetivo
Definir pruebas adicionales para verificar las reglas
de negocio de especialidades, consultorios y horarios.

## Caso 1: horario superpuesto
Intentar registrar dos horarios del mismo médico
que se superpongan en el mismo día.

Resultado esperado:
La API debe rechazar la operación según sus reglas
de validación.

Estado: pendiente de ejecución.

## Caso 2: horario inválido
Intentar registrar un horario cuya hora de inicio
sea posterior a la hora de finalización.

Resultado esperado:
La API debe rechazar los datos inválidos.

Estado: pendiente de ejecución.

## Caso 3: recurso inexistente
Consultar un horario con un identificador inexistente.

Resultado esperado:
HTTP 404 Not Found.

Estado: pendiente de ejecución.

## Caso 4: asignación de consultorio
Consultar disponibilidad de un médico sin consultorio
activo asignado.

Resultado observado:
La consulta devolvió turnos con roomIds vacío.

Estado: verificado.

## Conclusión
Se documentaron casos de validación para futuras
pruebas manuales y automatizadas del backend.



Mensaje del commit:
Adhelmar: documentar casos de validacion
