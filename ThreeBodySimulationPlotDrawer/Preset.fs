module Preset

open ThreeBodySimulation.Data

open System.IO
open System.Text.Json

type SimPreset = {
    Body1 : Body
    Body2 : Body
    Body3 : Body
    G : double
}

let jsonOptions = JsonSerializerOptions(WriteIndented = true)

let savePreset (path: string) (preset: SimPreset) : Result<unit, string> =
    try
        let json = JsonSerializer.Serialize(preset, jsonOptions)
        File.WriteAllText(path, json)
        Ok ()
    with
    | ex -> Error $"Failed to save file: {ex.Message}"

let loadPreset (path: string) : Result<SimPreset, string> =
    try
        let json = File.ReadAllText(path)
        let preset = JsonSerializer.Deserialize<SimPreset>(json, jsonOptions)
        Ok preset
    with
    | :? FileNotFoundException ->
        Error $"File not found: {path}"
    | :? JsonException as jsonEx ->
        Error $"JSON error: {jsonEx.Message}"
    | ex ->
        Error $"Unexpected error: {ex.Message}"