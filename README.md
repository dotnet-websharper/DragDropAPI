# WebSharper Drag and Drop Binding

[![NuGet](https://img.shields.io/nuget/v/WebSharper.DragDrop.svg)](https://www.nuget.org/packages/WebSharper.DragDrop)

An F# [WebSharper](https://websharper.com/) binding for the
[Drag and Drop API](https://developer.mozilla.org/en-US/docs/Web/API/HTML_Drag_and_Drop_API),
enabling drag-and-drop functionality in WebSharper applications with typed access to
`DataTransfer`, `DataTransferItem`, `DataTransferItemList`, `DragEvent` and the related
`dragstart` / `dragover` / `drop` events.

## Repository structure

| Project | Description |
| --- | --- |
| `WebSharper.DragDrop` | The WIG binding (`WebSharperProject=binding`, `netstandard2.0`). Defines `DragEvent`, `DataTransfer`, `DataTransferItem`, `DataTransferItemList` and the `DropEffect` / `EffectAllowed` enums. |
| `WebSharper.DragDrop.Extensions` | Hand-written `[<Inline>]` extensions (`WebSharperProject=library`, `netstandard2.0`). Adds `ondrag*` accessors to `HTMLElement`. |
| `WebSharper.DragDrop.Sample` | ASP.NET Core SPA sample (`net10.0`). Demonstrates the binding with WebSharper.UI template holes and Vars. |

## Live demos

- **GitHub Pages sample**: <https://dotnet-websharper.github.io/DragDropAPI/>
- **Source repository**: <https://github.com/dotnet-websharper/DragDropAPI>
- **WebSharper bindings**: <https://websharper.com/bindings>

## Installation

```sh
dotnet add package WebSharper.DragDrop
```

The package ships the `WebSharper.DragDrop` binding together with its
`WebSharper.DragDrop.Extensions` companion, which provides the `HTMLElement.ondrag*`
accessors used in the sample.

## Usage

The sample in [`WebSharper.DragDrop.Sample`](WebSharper.DragDrop.Sample) uses WebSharper.UI
template holes and Vars rather than direct DOM manipulation:

```fsharp
namespace WebSharper.DragDrop.Sample

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Templating
open WebSharper.DragDrop

[<JavaScript>]
module Client =
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
```

The matching template holes in `wwwroot/index.html` are `ws-attr="DragBoxAttrs"`,
`ws-attr="DropZoneAttrs"` and `ws-hole="DropText"`:

```html
<div id="main" ws-children-template="Main">
    <div>
        <div id="dragBox" draggable="true" ws-attr="DragBoxAttrs">Drag Me</div>
        <div id="dropZone" ws-attr="DropZoneAttrs" ws-hole="DropText">Drop Here</div>
    </div>
</div>
```

### Notes on the API

- **Browser compatibility**: the Drag and Drop API is supported in all modern browsers.
  Some features (for example `DataTransferItem.getAsFileSystemHandle`) may require fallbacks
  or feature detection in older browsers.
- **Security restrictions**: browsers restrict dragging and dropping external files. The
  `DataTransfer` object only exposes files during the `drop` event.
- **Styling**: custom styling is usually needed to provide visual feedback during drag
  operations; the sample uses a `.drop-element` class bound reactively with
  `Attr.DynamicClassPred`.
- **Obsolete overloads**: the binding keeps the legacy `DataTransfer.addElement` and
  `DataTransferItem.webkitGetAsEntry` members for compatibility.

## Running the sample

The sample is an ASP.NET Core SPA. From the repository root:

```sh
dotnet run --project WebSharper.DragDrop.Sample
```

Then open the URL printed by `dotnet run` (the launch profile uses
`http://localhost:51365`) and drag the red box onto the dashed drop zone. The drop zone
text changes to the transferred text and the zone turns light green.

## Building from source

```sh
dotnet build WebSharper.DragDrop.sln -c Release
```

The sample's compiled JavaScript is emitted under
`WebSharper.DragDrop.Sample/wwwroot/Content`.

## Deployment

The sample is deployed to GitHub Pages by
[`.github/workflows/ghpages.yml`](.github/workflows/ghpages.yml). The workflow builds the
sample in Release and publishes the contents of its `wwwroot` directory.

## License

Apache 2.0 — see [LICENSE.md](LICENSE.md).
