#!/bin/bash

user=se
site=yoanlecoq.com
mirror_opts=" --only-newer --exclude-glob=.git* "

case $1 in
pull)
    ;;
push)
    mirror_opts+=" --reverse "
    ;;
*)
    echo Usage: $0 "<pull|push> [--verbose] [--dry-run]"
    exit
    ;;
esac

for arg in $@; do
    case $arg in
    --verbose)
        mirror_opts+=" --verbose=3 "
        ;;
    --dry-run)
        mirror_opts+=" --dry-run "
        ;;
    esac
done

script="\
    set ssl:verify-certificate no; \
    set ftp:list-options -a; \
    set xfer:log-file lftp_assets.log; \
    set xfer:log true; \
    set xfer:backup-suffix bak; \
    set xfer:make-backup true; \
    set xfer:keep-backup true; \
"
script+=" mirror $mirror_opts "
for i in Textures Models Sounds Music; do
    script+=" --directory=$i "
done
script+="--target-directory=. && echo Done && exit; "

lftp "$site" -u "$user@$site" -e "$script"
