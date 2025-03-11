# WebSharper Drag and Drop API Binding

This repository provides an F# [WebSharper](https://websharper.com/) binding for the [Drag and Drop API](https://developer.mozilla.org/en-US/docs/Web/API/HTML_Drag_and_Drop_API), enabling seamless integration of drag-and-drop functionality into WebSharper applications.

## Repository Structure

The repository consists of two main projects:

1. **Binding Project**:

   - Contains the F# WebSharper binding for the Drag and Drop API.

2. **Sample Project**:
   - Demonstrates how to use the Drag and Drop API with WebSharper syntax.
   - Includes a GitHub Pages demo: [View Demo](https://dotnet-websharper.github.io/DragDropAPI/)

## Installation

To use this package in your WebSharper project, add the NuGet package:

```bash
   dotnet add package WebSharper.DragDrop
```

## Building

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) installed on your machine.

### Steps

1. Clone the repository:

   ```bash
   git clone https://github.com/dotnet-websharper/DragDrop.git
   cd DragDrop
   ```

2. Build the Binding Project:

   ```bash
   dotnet build WebSharper.DragDrop/WebSharper.DragDrop.fsproj
   ```

3. Build and Run the Sample Project:

   ```bash
   cd WebSharper.DragDrop.Sample
   dotnet build
   dotnet run
   ```

4. Open the sample project in your browser to see it in action.

## Example Usage

Below is an example of how to use the Drag and Drop API in a WebSharper project:

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
```

## Important Considerations

- **Browser Compatibility**: The Drag and Drop API is supported in most modern browsers, but some features may require fallback implementations.
- **Security Restrictions**: Some browsers impose security restrictions on dragging and dropping external files.
- **Styling**: Custom styling may be required for better visual feedback during drag operations.
