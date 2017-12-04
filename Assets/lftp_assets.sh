#!/bin/bash

user=se
site=yoanlecoq.com
mirror_opts=" --only-newer --exclude-glob=.git* --verbose=3 "
do_mirror=true
leave=true

case $1 in
login)
    do_mirror=false
    leave=false
    ;;
pull)
    do_mirror=true
    ;;
push)
    do_mirror=true
    mirror_opts+=" --reverse "
    ;;
*)
    echo Usage: $0 "<pull|push|login> [--dry-run] [--stay]"
    exit
    ;;
esac

for arg in $@; do
    case $arg in
    --dry-run)
        mirror_opts+=" --dry-run "
        ;;
    --stay)
        leave=false
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
if $do_mirror; then
    script+=" mirror $mirror_opts "
    for i in Textures Models Sounds Music; do
        script+=" --directory=$i "
    done
    script+="--target-directory=. && echo Done; "
fi
if $leave; then
    script+=" exit; "
fi

lftp "$site" -u "$user@$site" -e "$script"
