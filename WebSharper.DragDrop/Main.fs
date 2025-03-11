namespace WebSharper.DragDrop

open WebSharper
open WebSharper.JavaScript
open WebSharper.InterfaceGenerator

module Definition =

    module Enum = 
        let DropEffect = 
            Pattern.EnumStrings "DropEffect" [
                "copy"
                "move"
                "link"
                "none"
            ]

        let EffectAllowed = 
            Pattern.EnumStrings "EffectAllowed" [
                "none"
                "copy"
                "copyLink"
                "copyMove"
                "link"
                "linkMove"
                "move"
                "all"
                "uninitialized"
            ]

    let DataTransferItem =
        Class "DataTransferItem"
        |+> Instance [
            "kind" =? T<string>
            "type" =? T<string>

            "getAsFile" => T<unit> ^-> T<File> 
            "getAsFileSystemHandle" => T<unit> ^-> T<Promise<_>>[T<obj>]
            "getAsString" => (T<obj>?data ^-> T<unit>)?callbackFn ^-> T<unit>
            "webkitGetAsEntry" => T<unit> ^-> T<obj>
        ]

    let DataTransferItemList =
        Class "DataTransferItemList"
        |+> Instance [
            "length" =? T<int> // Read-only: Number of drag items in the list

            "add" => T<string>?data * T<string>?``type`` ^-> DataTransferItem
            "add" => T<File>?file ^-> DataTransferItem
            "remove" => T<int>?index ^-> T<unit>
            "clear" => T<unit> ^-> T<unit>
        ]

    let DataTransfer =
        Class "DataTransfer"
        |+> Static [
            Constructor T<unit> // Creates a new DataTransfer object
        ]
        |+> Instance [
            "dropEffect" =@ Enum.DropEffect // Gets or sets the drag-and-drop operation type            
            "effectAllowed" =@ Enum.EffectAllowed // Provides allowed operations            
            "files" =? !|T<File> // Read-only list of files available in the data transfer           
            "items" =? DataTransferItemList + !|DataTransferItem // Read-only list of all drag data items            
            "types" =? !|T<string> // Read-only array of formats set in the dragstart event

            "addElement" => T<Dom.Element>?element ^-> T<unit>
            "clearData" => !?T<string>?format  ^-> T<unit>
            "getData" => T<string>?format ^-> T<string>
            "setData" => T<string>?format * T<string>?data ^-> T<unit>
            "setDragImage" => T<Dom.Element>?imgElement * T<int>?xOffset * T<int>?yOffset ^-> T<unit>
        ]

    let DragEventInit = 
        Pattern.Config "DragEventInit" {
            Required = []
            Optional = [
                "dataTransfer", DataTransfer.Type
            ]
        }

    let DragEvent =
        Class "DragEvent"
        |=> Inherits T<Dom.MouseEvent>
        |+> Static [
            Constructor (T<string>?eventType * !?DragEventInit?eventInitDict)
        ]
        |+> Instance [
            "dataTransfer" =? DataTransfer
        ]

    let HTMLElement = 
        Class "HTMLElement" 
        |+> Instance [
            "ondrag" =@ T<unit> ^-> T<unit>
            |> ObsoleteWithMessage "Use OnDrag instead"
            "ondrag" =@ DragEvent ^-> T<unit>
            |> WithSourceName "OnDrag"

            "ondragend" =@ T<unit> ^-> T<unit>
            |> ObsoleteWithMessage "Use OnDragEnd instead"
            "ondragend" =@ DragEvent ^-> T<unit>
            |> WithSourceName "OnDragEnd"

            "ondragenter" =@ T<unit> ^-> T<unit>
            |> ObsoleteWithMessage "Use OnDragEnter instead"
            "ondragenter" =@ DragEvent ^-> T<unit>
            |> WithSourceName "OnDragEnter"

            "ondragleave" =@ T<unit> ^-> T<unit>
            |> ObsoleteWithMessage "Use OnDragLeave instead"
            "ondragleave" =@ DragEvent ^-> T<unit>
            |> WithSourceName "OnDragLeave"

            "ondragover" =@ T<unit> ^-> T<unit>
            |> ObsoleteWithMessage "Use OnDragOver instead"
            "ondragover" =@ DragEvent ^-> T<unit>
            |> WithSourceName "OnDragOver"

            "ondragstart" =@ T<unit> ^-> T<unit>
            |> ObsoleteWithMessage "Use OnDragStart instead"
            "ondragstart" =@ DragEvent ^-> T<unit>
            |> WithSourceName "OnDragStart"

            "ondrop" =@ T<unit> ^-> T<unit>
            |> ObsoleteWithMessage "Use OnDrop instead"
            "ondrop" =@ DragEvent ^-> T<unit>
            |> WithSourceName "OnDrop"
        ]

    let Assembly =
        Assembly [
            Namespace "WebSharper.DragDrop" [
                HTMLElement
                DragEvent
                DragEventInit
                DataTransfer
                DataTransferItemList
                DataTransferItem
                Enum.EffectAllowed
                Enum.DropEffect
            ]
        ]

[<Sealed>]
type Extension() =
    interface IExtension with
        member ext.Assembly =
            Definition.Assembly

[<assembly: Extension(typeof<Extension>)>]
do ()
