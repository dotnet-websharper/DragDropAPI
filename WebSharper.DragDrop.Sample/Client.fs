namespace WebSharper.DragDrop.Sample

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Templating
open WebSharper.DragDrop

[<JavaScript>]
module Client =
    // The templates are loaded from the DOM, so you just can edit index.html
    // and refresh your browser, no need to recompile unless you add or remove holes.
    type IndexTemplate = Template<"wwwroot/index.html", ClientLoad.FromDocument>

    let setupDragAndDrop () =
        let dragBox = JS.Document.GetElementById("dragBox")
        let dropZone = JS.Document.GetElementById("dropZone")

        // Drag Start: Set data to transfer
        dragBox.AddEventListener("dragstart", fun (event: Dom.Event) ->
            let dragEvent = event |> As<DragEvent>
            dragEvent.DataTransfer.SetData("text/plain", "Dropped!")
        )

        // Allow dropping by preventing default behavior
        dropZone.AddEventListener("dragover", fun (event: Dom.Event) ->
            event.PreventDefault()
        )

        // Handle drop event
        dropZone.AddEventListener("drop", fun (event: Dom.Event) ->
            event.PreventDefault()
            let dragEvent = event |> As<DragEvent>
            let data = dragEvent.DataTransfer.GetData("text/plain")
            dropZone.TextContent <- data
            dropZone.ClassList.Add("drop-element")
        )

    [<SPAEntryPoint>]
    let Main () =

        IndexTemplate.Main()
            .BoxInit(fun () -> 
                setupDragAndDrop()
            )
            .Doc()
        |> Doc.RunById "main"
