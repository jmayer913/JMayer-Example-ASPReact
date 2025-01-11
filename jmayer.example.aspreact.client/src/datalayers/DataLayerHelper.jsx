//The function shapes the validation result into a C# ServerSideValidationResult object. This is done
//because the server can send a ValidationProblemDetails object instead of a ServerSideValidationResult object;
//ASP.NET core checks data annotations and sends a ValidationProblemDetails object on failure.
export function shapeValidationResult(result) {
    if (!Array.isArray(result.errors)) {
        let newResult = {
            errors: []
        };

        //errors will be a dictionary where the property name is the key and the value is a list of error messages.
        //The loop will convert the dictionary into an array of objects where each object is a property name and error message.
        for (let key in result.errors) {
            for (const errorMessage of result.errors[key]) {
                newResult.errors.push({
                    propertyName: key,
                    errorMessage: errorMessage
                });
            }
        }

        result = newResult;
    }

    return result;
}