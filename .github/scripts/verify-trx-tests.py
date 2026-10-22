import sys
import xml.etree.ElementTree as ET
from pathlib import Path


def main() -> int:
    if len(sys.argv) != 2:
        print("Usage: verify-trx-tests.py <path-to-trx>", file=sys.stderr)
        return 2

    trx_path = Path(sys.argv[1])
    if not trx_path.is_file():
        print(f"Test results file was not created: {trx_path}", file=sys.stderr)
        return 1

    root = ET.parse(trx_path).getroot()
    counters = next((element for element in root.iter() if element.tag.endswith("Counters")), None)
    total = 0 if counters is None else int(counters.attrib.get("total", counters.attrib.get("Total", "0")))

    if total < 1:
        print(f"No tests were discovered in {trx_path}.", file=sys.stderr)
        return 1

    print(f"Verified {total} discovered test(s) in {trx_path}.")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
