namespace WebSharper.DragDrop

open WebSharper
open WebSharper.JavaScript

[<JavaScript; AutoOpen>]
module Extensions =

    type HTMLElement with

        [<Inline "$this.ondrag">]
        member this.OnDrag with get(): (DragEvent -> unit) = ignore
        [<Inline "$this.ondrag = $callback">]
        member this.OnDrag with set(callback: DragEvent -> unit) = ()

        [<Inline "$this.ondragend">]
        member this.OnDragEnd with get(): (DragEvent -> unit) = ignore
        [<Inline "$this.ondragend = $callback">]
        member this.OnDragEnd with set(callback: DragEvent -> unit) = ()

        [<Inline "$this.ondragenter">]
        member this.OnDragEnter with get(): (DragEvent -> unit) = ignore
        [<Inline "$this.ondragenter = $callback">]
        member this.OnDragEnter with set(callback: DragEvent -> unit) = ()

        [<Inline "$this.ondragleave">]
        member this.OnDragLeave with get(): (DragEvent -> unit) = ignore
        [<Inline "$this.ondragleave = $callback">]
        member this.OnDragLeave with set(callback: DragEvent -> unit) = ()

        [<Inline "$this.ondragover">]
        member this.OnDragOver with get(): (DragEvent -> unit) = ignore
        [<Inline "$this.ondragover = $callback">]
        member this.OnDragOver with set(callback: DragEvent -> unit) = ()

        [<Inline "$this.ondragstart">]
        member this.OnDragStart with get(): (DragEvent -> unit) = ignore
        [<Inline "$this.ondragstart = $callback">]
        member this.OnDragStart with set(callback: DragEvent -> unit) = ()

        [<Inline "$this.ondrop">]
        member this.OnDrop with get(): (DragEvent -> unit) = ignore
        [<Inline "$this.ondrop = $callback">]
        member this.OnDrop with set(callback: DragEvent -> unit) = ()
