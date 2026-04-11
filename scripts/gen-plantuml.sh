#!/usr/bin/env bash
set -euo pipefail

# Run from repository root (or from anywhere — script cds to root).
ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$ROOT"

command -v puml-gen >/dev/null 2>&1 || {
  echo "puml-gen not found. Install: dotnet tool install --global PlantUmlClassDiagramGenerator" >&2
  exit 1
}

BASE="RpgGame"
OUT="diagrams/plantuml"
OPTS="-dir -public -excludePaths bin,obj"

mkdir -p "$OUT"/{core,character,combat,input,items,generation,renderer,tiles}

puml-gen "$BASE/Core"       "$OUT/core"       $OPTS
puml-gen "$BASE/Character"  "$OUT/character"  $OPTS
puml-gen "$BASE/Combat"     "$OUT/combat"     $OPTS
puml-gen "$BASE/Input"      "$OUT/input"      $OPTS
puml-gen "$BASE/Items"      "$OUT/items"      $OPTS
puml-gen "$BASE/Generation" "$OUT/generation" $OPTS
puml-gen "$BASE/Renderer"   "$OUT/renderer"   $OPTS
puml-gen "$BASE/Tiles"      "$OUT/tiles"      $OPTS

echo "PlantUML sources written under $OUT/"
