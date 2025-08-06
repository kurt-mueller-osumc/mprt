namespace Shared

open System

type Todo = { Id: Guid; Description: string }

module Todo =
    let isValid (description: string) =
        String.IsNullOrWhiteSpace description |> not

    let create (description: string) = {
        Id = Guid.NewGuid()
        Description = description
    }

type ITodosApi = {
    getTodos: unit -> Async<Todo list>
    addTodo: Todo -> Async<Todo list>
}

module Domain =
    module Patient =
        type MedicalId = internal MedicalId of string

        type EstimatedDeliveryDate = internal EstimatedDeliveryDate of DateTime

module Input =
    module Patient =
        type MedicalId =
            | MedicalId of string

            /// <summary>
            /// Validates the MedicalId based on specific business rules.
            /// </summary>
            /// <remarks>
            /// The validation checks for the following conditions:
            /// <list type="bullet">
            /// <item>The ID is not null or whitespace.</item>
            /// <item>The ID is exactly 12 characters long.</item>
            /// <item>The ID contains only digits.</item>
            /// </list>
            /// </remarks>
            /// <returns>
            /// A <c>Result</c> containing the validated <c>Domain.Patient.MedicalId</c> on success,
            /// or a list of error messages on failure.
            /// </returns>
            member this.Validate() : Result<Domain.Patient.MedicalId, string list> =
                match this with
                | MedicalId id ->
                    let errors = ResizeArray<string>()

                    if String.IsNullOrWhiteSpace id then
                        errors.Add "Medical ID cannot be empty"
                    else
                        if id.Length <> 12 then
                            errors.Add "Medical ID must be exactly 12 characters long"

                        if not (id |> Seq.forall Char.IsDigit) then
                            errors.Add "Medical ID must contain only digits"

                    if errors.Count = 0 then
                        Ok <| Domain.Patient.MedicalId id
                    else
                        Error(errors |> List.ofSeq)

        type EstimatedDeliveryDate =
            | EstimatedDeliveryDate of DateTime

            member this.Validate() : Result<Domain.Patient.EstimatedDeliveryDate, string list> =
                match this with
                | EstimatedDeliveryDate date ->
                    let errors = ResizeArray<string>()
                    let maxFutureDate = DateTime.Now.AddDays 280.0

                    if date < DateTime.Now then
                        errors.Add "Estimated delivery date must be in the future"

                    else if date > maxFutureDate then
                        errors.Add "Estimated delivery date cannot be more than 280 days in the future"

                    if errors.Count = 0 then
                        Ok <| Domain.Patient.EstimatedDeliveryDate date
                    else
                        Error(errors |> List.ofSeq)