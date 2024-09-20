module JsonConverter

open System
open System.Text.Json
open System.Text.Json.Serialization


type DateTimeConverter() =
    inherit JsonConverter<DateTime>()
    override _.Read(reader, _, _) =
        let dateStr = reader.GetString()
        match DateTime.TryParse(dateStr) with
        | true, date -> date
        | _ -> raise (JsonException("Invalid date format"))

    override _.Write(writer, value, _) =
        writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss"))

