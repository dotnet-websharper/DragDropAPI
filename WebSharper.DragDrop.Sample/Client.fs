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

    [<SPAEntryPoint>]
    let Main () =
        let dropText = Var.Create "Drop Here"
        let isDropped = Var.Create false

        // Attach the dragstart handler without reaching for the DOM directly.
        let dragBoxAttrs =
            Attr.OnAfterRender(fun el ->
                el.AddEventListener("dragstart", fun (event: Dom.Event) ->
                    let dragEvent = event |> As<DragEvent>
                    dragEvent.DataTransfer.SetData("text/plain", "Dropped!")
                )
            )

        // Allow dropping, and react to the drop with WebSharper.UI Vars.
        let dropZoneAttrs =
            Attr.Append
                (Attr.OnAfterRender(fun el ->
                    el.AddEventListener("dragover", fun (event: Dom.Event) ->
                        event.PreventDefault()
                    )
                    el.AddEventListener("drop", fun (event: Dom.Event) ->
                        event.PreventDefault()
                        let dragEvent = event |> As<DragEvent>
                        let data = dragEvent.DataTransfer.GetData("text/plain")
                        dropText.Value <- data
                        isDropped.Value <- true
                    )
                ))
                (Attr.DynamicClassPred "drop-element" isDropped.View)

        IndexTemplate.Main()
            .DragBoxAttrs(dragBoxAttrs)
            .DropZoneAttrs(dropZoneAttrs)
            .DropText(Doc.TextView(dropText.View))
            .Doc()
        |> Doc.RunById "main"
