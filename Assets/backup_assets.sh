#!/bin/bash

for i in `find Textures Models Sounds Music -type f`; do
    if [[ $i == *.meta ]] || [[ $i == *.git* ]]; then
        continue
    fi
    o="LocalBackups/$i"
    mkdir -p `dirname $o` && cp --update --verbose $i $o
done

