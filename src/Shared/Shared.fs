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

module Input =
    module Patient =
        type MedicalId =
            | MedicalId of string

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