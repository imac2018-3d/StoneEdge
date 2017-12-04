#!/bin/bash

# NOTE: To remove all backups, just do
# find . -name "*.bak" -delete

me=`whoami`

dry_run=false
if [[ $1 == --dry-run ]]; then
    dry_run=true
    echo "This is what would happen: "
fi

for i in `find Textures Models Sounds Music -type f`; do
    if [[ $i == *.meta ]] || [[ $i == *.git* ]] || [[ $i == *.bak ]]; then
        continue
    fi
    if $dry_run; then
        echo "$i --> $i.$me.bak"
    else
        cp --update --verbose $i $i.$me.bak
    fi
done

