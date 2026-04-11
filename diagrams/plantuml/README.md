# PlantUML class diagrams

These `.puml` files are generated from the C# sources with [PlantUmlClassDiagramGenerator](https://github.com/pierre3/PlantUmlClassDiagramGenerator) (`puml-gen`).

## When to regenerate

After **large structural changes** (new namespaces/folders, big refactors, or many new public types), run the generator so diagrams stay accurate before you push.

## How to regenerate

From the repository root:

```bash
./scripts/gen-plantuml.sh
```

Prerequisite: install the global tool once:

```bash
dotnet tool install --global PlantUmlClassDiagramGenerator
```

Ensure `~/.dotnet/tools` is on your `PATH` if `puml-gen` is not found.

## Layout

Outputs are split by area under this directory (`core`, `character`, `combat`, `input`, `items`, `generation`, `renderer`, `tiles`) to keep diagrams smaller than a single full-project diagram.

To turn `.puml` into images locally, use [PlantUML](https://plantuml.com/) (JAR, Docker, or an editor extension).
