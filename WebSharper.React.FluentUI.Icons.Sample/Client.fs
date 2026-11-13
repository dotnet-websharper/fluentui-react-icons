namespace WebSharper.FluentUI.IconGallery

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Templating
open WebSharper.React
open WebSharper.FluentUI.React

[<JavaScript>]
module Client =

    /// The whole document is the template, so editing index.html and
    /// refreshing the browser is enough.
    type IndexTemplate = Template<"index.html", ClientLoad.FromDocument>

    /// Keep in sync with the `@fluentui/react-icons` version pinned in
    /// WebSharper.React.FluentUi.Icons.fsproj, package.json and
    /// tools/gen-icons.py's output.
    let npmVersion = "2.0.345"

    /// The number of icons the binding exposes (one per export of the npm
    /// package; run `python3 tools/gen-icons.py --check` to verify).
    let iconCount = 6182

    /// Keep in sync with the inline theme script in index.html.
    let themeKey = "websharper-fluentui-icons-theme"
    let theme = Var.Create "light"
    let themeLabel = Var.Create "🌙 Dark mode"
    let status = Var.Create ""
    let reactVersion = Var.Create "-"

    /// A React prop. The binding's helpers take `seq<string * obj>`.
    let private prop (k: string) (v: obj) = (k, v)

    let applyTheme () =
        JS.Document.DocumentElement.SetAttribute("data-theme", theme.Value)
        themeLabel.Value <-
            if theme.Value = "dark" then "☀️ Light mode" else "🌙 Dark mode"

    let toggleTheme () =
        theme.Value <- (if theme.Value = "dark" then "light" else "dark")
        try JS.Window.LocalStorage.SetItem(themeKey, theme.Value) with _ -> ()
        applyTheme ()

    // ------------------------------------------------------------------
    // The icons the gallery shows. `Icons.Helpers.*` are the binding's
    // element helpers: each takes `seq<string * obj>` and renders the icon.
    // ------------------------------------------------------------------

    type IconPair =
        {
            /// The base name; the npm package exports `<Name>Filled` and `<Name>Regular`.
            Name: string
            Filled: seq<string * obj> -> React.Element
            Regular: seq<string * obj> -> React.Element
        }

    type ColorIcon =
        {
            Name: string
            Render: seq<string * obj> -> React.Element
        }

    let private catalog : IconPair list = [
            { Name = "Add"; Filled = Icons.Helpers.addFilled; Regular = Icons.Helpers.addRegular }
            { Name = "ArrowLeft"; Filled = Icons.Helpers.arrowLeftFilled; Regular = Icons.Helpers.arrowLeftRegular }
            { Name = "ArrowRight"; Filled = Icons.Helpers.arrowRightFilled; Regular = Icons.Helpers.arrowRightRegular }
            { Name = "ArrowUp"; Filled = Icons.Helpers.arrowUpFilled; Regular = Icons.Helpers.arrowUpRegular }
            { Name = "ArrowDown"; Filled = Icons.Helpers.arrowDownFilled; Regular = Icons.Helpers.arrowDownRegular }
            { Name = "ArrowUpRight"; Filled = Icons.Helpers.arrowUpRightFilled; Regular = Icons.Helpers.arrowUpRightRegular }
            { Name = "Search"; Filled = Icons.Helpers.searchFilled; Regular = Icons.Helpers.searchRegular }
            { Name = "Settings"; Filled = Icons.Helpers.settingsFilled; Regular = Icons.Helpers.settingsRegular }
            { Name = "Home"; Filled = Icons.Helpers.homeFilled; Regular = Icons.Helpers.homeRegular }
            { Name = "Person"; Filled = Icons.Helpers.personFilled; Regular = Icons.Helpers.personRegular }
            { Name = "People"; Filled = Icons.Helpers.peopleFilled; Regular = Icons.Helpers.peopleRegular }
            { Name = "Mail"; Filled = Icons.Helpers.mailFilled; Regular = Icons.Helpers.mailRegular }
            { Name = "Calendar"; Filled = Icons.Helpers.calendarFilled; Regular = Icons.Helpers.calendarRegular }
            { Name = "Clock"; Filled = Icons.Helpers.clockFilled; Regular = Icons.Helpers.clockRegular }
            { Name = "Document"; Filled = Icons.Helpers.documentFilled; Regular = Icons.Helpers.documentRegular }
            { Name = "Folder"; Filled = Icons.Helpers.folderFilled; Regular = Icons.Helpers.folderRegular }
            { Name = "Image"; Filled = Icons.Helpers.imageFilled; Regular = Icons.Helpers.imageRegular }
            { Name = "Camera"; Filled = Icons.Helpers.cameraFilled; Regular = Icons.Helpers.cameraRegular }
            { Name = "Video"; Filled = Icons.Helpers.videoFilled; Regular = Icons.Helpers.videoRegular }
            { Name = "Play"; Filled = Icons.Helpers.playFilled; Regular = Icons.Helpers.playRegular }
            { Name = "Pause"; Filled = Icons.Helpers.pauseFilled; Regular = Icons.Helpers.pauseRegular }
            { Name = "Stop"; Filled = Icons.Helpers.stopFilled; Regular = Icons.Helpers.stopRegular }
            { Name = "Mic"; Filled = Icons.Helpers.micFilled; Regular = Icons.Helpers.micRegular }
            { Name = "Phone"; Filled = Icons.Helpers.phoneFilled; Regular = Icons.Helpers.phoneRegular }
            { Name = "Chat"; Filled = Icons.Helpers.chatFilled; Regular = Icons.Helpers.chatRegular }
            { Name = "Comment"; Filled = Icons.Helpers.commentFilled; Regular = Icons.Helpers.commentRegular }
            { Name = "Send"; Filled = Icons.Helpers.sendFilled; Regular = Icons.Helpers.sendRegular }
            { Name = "Share"; Filled = Icons.Helpers.shareFilled; Regular = Icons.Helpers.shareRegular }
            { Name = "Link"; Filled = Icons.Helpers.linkFilled; Regular = Icons.Helpers.linkRegular }
            { Name = "Copy"; Filled = Icons.Helpers.copyFilled; Regular = Icons.Helpers.copyRegular }
            { Name = "Delete"; Filled = Icons.Helpers.deleteFilled; Regular = Icons.Helpers.deleteRegular }
            { Name = "Edit"; Filled = Icons.Helpers.editFilled; Regular = Icons.Helpers.editRegular }
            { Name = "Save"; Filled = Icons.Helpers.saveFilled; Regular = Icons.Helpers.saveRegular }
            { Name = "Print"; Filled = Icons.Helpers.printFilled; Regular = Icons.Helpers.printRegular }
            { Name = "Cloud"; Filled = Icons.Helpers.cloudFilled; Regular = Icons.Helpers.cloudRegular }
            { Name = "Database"; Filled = Icons.Helpers.databaseFilled; Regular = Icons.Helpers.databaseRegular }
            { Name = "Server"; Filled = Icons.Helpers.serverFilled; Regular = Icons.Helpers.serverRegular }
            { Name = "Code"; Filled = Icons.Helpers.codeFilled; Regular = Icons.Helpers.codeRegular }
            { Name = "Bug"; Filled = Icons.Helpers.bugFilled; Regular = Icons.Helpers.bugRegular }
            { Name = "Wrench"; Filled = Icons.Helpers.wrenchFilled; Regular = Icons.Helpers.wrenchRegular }
            { Name = "Shield"; Filled = Icons.Helpers.shieldFilled; Regular = Icons.Helpers.shieldRegular }
            { Name = "Key"; Filled = Icons.Helpers.keyFilled; Regular = Icons.Helpers.keyRegular }
            { Name = "Eye"; Filled = Icons.Helpers.eyeFilled; Regular = Icons.Helpers.eyeRegular }
            { Name = "EyeOff"; Filled = Icons.Helpers.eyeOffFilled; Regular = Icons.Helpers.eyeOffRegular }
            { Name = "Star"; Filled = Icons.Helpers.starFilled; Regular = Icons.Helpers.starRegular }
            { Name = "Heart"; Filled = Icons.Helpers.heartFilled; Regular = Icons.Helpers.heartRegular }
            { Name = "Flag"; Filled = Icons.Helpers.flagFilled; Regular = Icons.Helpers.flagRegular }
            { Name = "Pin"; Filled = Icons.Helpers.pinFilled; Regular = Icons.Helpers.pinRegular }
            { Name = "Bookmark"; Filled = Icons.Helpers.bookmarkFilled; Regular = Icons.Helpers.bookmarkRegular }
            { Name = "Filter"; Filled = Icons.Helpers.filterFilled; Regular = Icons.Helpers.filterRegular }
            { Name = "Grid"; Filled = Icons.Helpers.gridFilled; Regular = Icons.Helpers.gridRegular }
            { Name = "List"; Filled = Icons.Helpers.listFilled; Regular = Icons.Helpers.listRegular }
            { Name = "Table"; Filled = Icons.Helpers.tableFilled; Regular = Icons.Helpers.tableRegular }
            { Name = "ChartMultiple"; Filled = Icons.Helpers.chartMultipleFilled; Regular = Icons.Helpers.chartMultipleRegular }
            { Name = "Alert"; Filled = Icons.Helpers.alertFilled; Regular = Icons.Helpers.alertRegular }
            { Name = "Info"; Filled = Icons.Helpers.infoFilled; Regular = Icons.Helpers.infoRegular }
            { Name = "Warning"; Filled = Icons.Helpers.warningFilled; Regular = Icons.Helpers.warningRegular }
            { Name = "ErrorCircle"; Filled = Icons.Helpers.errorCircleFilled; Regular = Icons.Helpers.errorCircleRegular }
            { Name = "Checkmark"; Filled = Icons.Helpers.checkmarkFilled; Regular = Icons.Helpers.checkmarkRegular }
            { Name = "Dismiss"; Filled = Icons.Helpers.dismissFilled; Regular = Icons.Helpers.dismissRegular }
            { Name = "Question"; Filled = Icons.Helpers.questionFilled; Regular = Icons.Helpers.questionRegular }
            { Name = "Lightbulb"; Filled = Icons.Helpers.lightbulbFilled; Regular = Icons.Helpers.lightbulbRegular }
            { Name = "Rocket"; Filled = Icons.Helpers.rocketFilled; Regular = Icons.Helpers.rocketRegular }
            { Name = "Target"; Filled = Icons.Helpers.targetFilled; Regular = Icons.Helpers.targetRegular }
            { Name = "Trophy"; Filled = Icons.Helpers.trophyFilled; Regular = Icons.Helpers.trophyRegular }
            { Name = "Gift"; Filled = Icons.Helpers.giftFilled; Regular = Icons.Helpers.giftRegular }
            { Name = "Cart"; Filled = Icons.Helpers.cartFilled; Regular = Icons.Helpers.cartRegular }
            { Name = "Wallet"; Filled = Icons.Helpers.walletFilled; Regular = Icons.Helpers.walletRegular }
            { Name = "Money"; Filled = Icons.Helpers.moneyFilled; Regular = Icons.Helpers.moneyRegular }
            { Name = "Building"; Filled = Icons.Helpers.buildingFilled; Regular = Icons.Helpers.buildingRegular }
            { Name = "Globe"; Filled = Icons.Helpers.globeFilled; Regular = Icons.Helpers.globeRegular }
            { Name = "Bluetooth"; Filled = Icons.Helpers.bluetoothFilled; Regular = Icons.Helpers.bluetoothRegular }
            { Name = "Power"; Filled = Icons.Helpers.powerFilled; Regular = Icons.Helpers.powerRegular }
            { Name = "WeatherSunny"; Filled = Icons.Helpers.weatherSunnyFilled; Regular = Icons.Helpers.weatherSunnyRegular }
            { Name = "WeatherMoon"; Filled = Icons.Helpers.weatherMoonFilled; Regular = Icons.Helpers.weatherMoonRegular }
            { Name = "WeatherCloudy"; Filled = Icons.Helpers.weatherCloudyFilled; Regular = Icons.Helpers.weatherCloudyRegular }
            { Name = "WeatherRain"; Filled = Icons.Helpers.weatherRainFilled; Regular = Icons.Helpers.weatherRainRegular }
            { Name = "TreeDeciduous"; Filled = Icons.Helpers.treeDeciduousFilled; Regular = Icons.Helpers.treeDeciduousRegular }
            { Name = "AnimalDog"; Filled = Icons.Helpers.animalDogFilled; Regular = Icons.Helpers.animalDogRegular }
            { Name = "Food"; Filled = Icons.Helpers.foodFilled; Regular = Icons.Helpers.foodRegular }
            { Name = "Book"; Filled = Icons.Helpers.bookFilled; Regular = Icons.Helpers.bookRegular }
            { Name = "Notebook"; Filled = Icons.Helpers.notebookFilled; Regular = Icons.Helpers.notebookRegular }
            { Name = "Library"; Filled = Icons.Helpers.libraryFilled; Regular = Icons.Helpers.libraryRegular }
            { Name = "Briefcase"; Filled = Icons.Helpers.briefcaseFilled; Regular = Icons.Helpers.briefcaseRegular }
            { Name = "Map"; Filled = Icons.Helpers.mapFilled; Regular = Icons.Helpers.mapRegular }
            { Name = "Navigation"; Filled = Icons.Helpers.navigationFilled; Regular = Icons.Helpers.navigationRegular }
            { Name = "Airplane"; Filled = Icons.Helpers.airplaneFilled; Regular = Icons.Helpers.airplaneRegular }
            { Name = "VehicleCar"; Filled = Icons.Helpers.vehicleCarFilled; Regular = Icons.Helpers.vehicleCarRegular }
            { Name = "VehicleBus"; Filled = Icons.Helpers.vehicleBusFilled; Regular = Icons.Helpers.vehicleBusRegular }
            { Name = "Sparkle"; Filled = Icons.Helpers.sparkleFilled; Regular = Icons.Helpers.sparkleRegular }
            { Name = "PaintBrush"; Filled = Icons.Helpers.paintBrushFilled; Regular = Icons.Helpers.paintBrushRegular }
            { Name = "Pen"; Filled = Icons.Helpers.penFilled; Regular = Icons.Helpers.penRegular }
            { Name = "TextBold"; Filled = Icons.Helpers.textBoldFilled; Regular = Icons.Helpers.textBoldRegular }
            { Name = "TextItalic"; Filled = Icons.Helpers.textItalicFilled; Regular = Icons.Helpers.textItalicRegular }
            { Name = "Location"; Filled = Icons.Helpers.locationFilled; Regular = Icons.Helpers.locationRegular }
            { Name = "CalendarClock"; Filled = Icons.Helpers.calendarClockFilled; Regular = Icons.Helpers.calendarClockRegular }
            { Name = "Timer"; Filled = Icons.Helpers.timerFilled; Regular = Icons.Helpers.timerRegular }
            { Name = "Hourglass"; Filled = Icons.Helpers.hourglassFilled; Regular = Icons.Helpers.hourglassRegular }
    ]

    let private colorCatalog : ColorIcon list = [
            { Name = "CalendarColor"; Render = Icons.Helpers.calendarColor }
            { Name = "CameraColor"; Render = Icons.Helpers.cameraColor }
            { Name = "MailColor"; Render = Icons.Helpers.mailColor }
            { Name = "TableColor"; Render = Icons.Helpers.tableColor }
            { Name = "CodeColor"; Render = Icons.Helpers.codeColor }
            { Name = "DataBarVerticalAscendingColor"; Render = Icons.Helpers.dataBarVerticalAscendingColor }
            { Name = "DatabaseColor"; Render = Icons.Helpers.databaseColor }
            { Name = "NotebookColor"; Render = Icons.Helpers.notebookColor }
            { Name = "DocumentColor"; Render = Icons.Helpers.documentColor }
            { Name = "SettingsColor"; Render = Icons.Helpers.settingsColor }
            { Name = "PaintBrushColor"; Render = Icons.Helpers.paintBrushColor }
            { Name = "PersonColor"; Render = Icons.Helpers.personColor }
            { Name = "BriefcaseColor"; Render = Icons.Helpers.briefcaseColor }
            { Name = "SearchVisualColor"; Render = Icons.Helpers.searchVisualColor }
            { Name = "BotColor"; Render = Icons.Helpers.botColor }
            { Name = "CalendarClockColor"; Render = Icons.Helpers.calendarClockColor }
            { Name = "DocumentTextColor"; Render = Icons.Helpers.documentTextColor }
            { Name = "RibbonColor"; Render = Icons.Helpers.ribbonColor }
            { Name = "PersonHeartColor"; Render = Icons.Helpers.personHeartColor }
            { Name = "BoardColor"; Render = Icons.Helpers.boardColor }
    ]

    // ------------------------------------------------------------------
    // The React gallery
    // ------------------------------------------------------------------

    let private section (title: string) (note: string) (content: React.Element) =
        Html.section [ prop "className" "section" ] [
            Html.h2 [ prop "className" "section-title" ] [ Html.text title ]
            Html.p [ prop "className" "section-note" ] [ Html.text note ]
            content
        ]

    let private iconProps (size: int) (fill: string) (name: string) =
        [
            prop "fontSize" size
            prop "title" name
            yield! (if fill = "" then [] else [ prop "primaryFill" fill ])
        ]

    let private grid (size: int) (fill: string) (variant: string) (entries: IconPair list) =
        let variants (e: IconPair) =
            match variant with
            | "filled" -> [ ("filled", e.Filled) ]
            | "regular" -> [ ("regular", e.Regular) ]
            | _ -> [ ("filled", e.Filled); ("regular", e.Regular) ]

        Html.div [ prop "className" "grid" ] [
            for e in entries do
                yield Html.div [
                    prop "key" e.Name
                    prop "className" "tile"
                    prop "title" e.Name
                ] [
                    yield Html.div [ prop "className" "tile-icons" ] [
                        for (v, icon) in variants e do
                            yield icon (
                                prop "key" v :: iconProps size fill e.Name)
                    ]
                    yield Html.div [ prop "className" "tile-name" ] [ Html.text e.Name ]
                    if variant = "both" then
                        yield Html.div [ prop "className" "tile-variant" ] [ Html.text "Filled / Regular" ]
                ]
        ]

    let private colorGrid (size: int) (entries: ColorIcon list) =
        Html.div [ prop "className" "grid" ] [
            for e in entries do
                yield Html.div [
                    prop "key" e.Name
                    prop "className" "tile"
                    prop "title" e.Name
                ] [
                    yield Html.div [ prop "className" "tile-icons" ] [
                        yield e.Render [
                            prop "fontSize" size
                            prop "title" e.Name
                            // Scope the ids of a Color icon's gradients/clip paths,
                            // which are otherwise global to the page.
                            prop "idPrefix" ("gallery-" + e.Name)
                        ]
                    ]
                    yield Html.div [ prop "className" "tile-name" ] [ Html.text e.Name ]
                ]
        ]

    let private sizesRow size =
        Html.div [ prop "className" "tiles-row" ] [
            for s in [ 16; 20; 24; 32; 48; 64 ] do
                yield Html.div [ prop "key" (string s); prop "className" "row-item" ] [
                    yield Icons.Helpers.addFilled (iconProps s "" "Add")
                    yield Html.div [ prop "className" "row-caption" ] [ Html.text (string s + "px") ]
                ]
        ]

    let private tintRow size =
        let fills = [ "Current color", ""; "Brand", "#0078d4"; "Teal", "#00b7c3"; "Magenta", "#e3008c"; "Gold", "#f7a600" ]
        Html.div [ prop "className" "tiles-row" ] [
            for (label, fill) in fills do
                yield Html.div [ prop "key" label; prop "className" "row-item" ] [
                    yield Icons.Helpers.alertFilled (iconProps size fill "Alert")
                    yield Html.div [ prop "className" "row-caption" ] [ Html.text label ]
                ]
        ]

    let private App () =
        let query, setQuery = React.UseState ""
        let variant, setVariant = React.UseState "both"
        let size, setSize = React.UseState 28
        let fill, setFill = React.UseState ""

        let q = query.Trim().ToLower()
        let matching =
            catalog
            |> List.filter (fun e -> q = "" || e.Name.ToLower().IndexOf(q) >= 0)

        let chip (value: string) (label: string) =
            Html.button [
                prop "key" value
                prop "className" (if variant = value then "chip chip-active" else "chip")
                Html.on.click (fun (_: MouseEvent) -> setVariant.Invoke value)
            ] [ Html.text label ]

        let swatch (value: string) (label: string) (css: string) =
            let cls =
                if fill = value then "swatch " + css + " swatch-active" else "swatch " + css
            Html.button [
                prop "key" label
                prop "title" label
                prop "aria-label" label
                prop "className" cls
                Html.on.click (fun (_: MouseEvent) -> setFill.Invoke value)
            ] []

        let onSearch (e: SyntheticEvent) =
            setQuery.Invoke (As<HTMLInputElement> e.Target).Value

        let onSize (e: SyntheticEvent) =
            setSize.Invoke (int (As<HTMLInputElement> e.Target).Value)

        Html.div [ prop "className" "gallery" ] [
            yield Html.div [ prop "className" "controls" ] [
                yield Html.input [
                    prop "className" "search"
                    prop "type" "search"
                    prop "placeholder" "Filter icons by name…"
                    prop "value" query
                    Html.on.input onSearch
                ] []
                yield Html.div [ prop "className" "control-group" ] [
                    yield Html.span [ prop "className" "control-label" ] [ Html.text "Variant" ]
                    yield chip "both" "Both"
                    yield chip "filled" "Filled"
                    yield chip "regular" "Regular"
                ]
                yield Html.div [ prop "className" "control-group" ] [
                    yield Html.span [ prop "className" "control-label" ] [ Html.text "Size" ]
                    yield Html.input [
                        prop "className" "slider"
                        prop "type" "range"
                        prop "min" 16
                        prop "max" 64
                        prop "step" 2
                        prop "value" size
                        Html.on.input onSize
                    ] []
                    yield Html.span [ prop "className" "row-caption" ] [ Html.text (string size + "px") ]
                ]
                yield Html.div [ prop "className" "control-group" ] [
                    yield Html.span [ prop "className" "control-label" ] [ Html.text "Tint" ]
                    yield swatch "" "Default" "swatch-default"
                    yield swatch "#0078d4" "Brand" "swatch-brand"
                    yield swatch "#00b7c3" "Teal" "swatch-teal"
                    yield swatch "#e3008c" "Magenta" "swatch-magenta"
                    yield swatch "#f7a600" "Gold" "swatch-gold"
                ]
                yield Html.span [ prop "className" "count" ] [
                    Html.text (string matching.Length + " of " + string catalog.Length + " icon pairs")
                ]
            ]
            yield
                if matching.IsEmpty then
                    Html.p [ prop "className" "empty" ] [
                        Html.text ("No icons match \"" + query + "\".")
                    ]
                else
                    section "Filled and Regular variants"
                        "Each name has a filled and a regular outline variant; the switcher picks one or shows both."
                        (grid size fill variant matching)
            yield
                section "Color icons"
                    "The Color variants ship their own light and dark palettes inside the SVG. The sample passes a unique idPrefix per tile so their gradient ids do not collide."
                    (colorGrid size colorCatalog)
            yield
                section "Sizing"
                    "Icons are 1em, so fontSize scales them; primaryFill tints the monotone variants and defaults to currentColor."
                    (sizesRow size)
            yield
                section "Tinting"
                    "primaryFill accepts any CSS color, including a theme variable resolved at render time."
                    (tintRow size)
        ]

    [<SPAEntryPoint>]
    let Main () =
        // Restore a previously chosen theme (light is the default).
        let saved = try JS.Window.LocalStorage.GetItem themeKey with _ -> null
        if not (isNull saved) && (saved = "dark" || saved = "light") then
            theme.Value <- saved
        applyTheme ()

        try
            reactVersion.Value <- React.Version
        with _ -> ()

        // Bind the page chrome first; `.Bind()` hydrates the whole document body.
        IndexTemplate()
            .themeLabel(themeLabel.V)
            .npmVersion(npmVersion)
            .iconCount(string iconCount)
            .reactVersion(reactVersion.V)
            .status(status.V)
            .ToggleTheme(fun _ -> toggleTheme ())
            .Bind()

        // The DOM is ready now, so mount the React gallery into its container.
        match JS.Document.GetElementById "icons-root" with
        | null ->
            status.Value <- "The #icons-root container was not found."
        | root ->
            try
                ReactHelpers.Mount root (WebSharper.React.Helpers.Helpers.ofFunction App () [])
                status.Value <-
                    "Mounted " + string catalog.Length + " icon pairs and "
                    + string colorCatalog.Length + " color icons from @fluentui/react-icons "
                    + npmVersion + " through React " + React.Version
            with ex ->
                status.Value <- "Could not mount the gallery: " + ex.Message