#!/bin/bash

user=se
site=yoanlecoq.com
mirror_opts=""
do_backup=true
do_mirror=true
leave=true

case $1 in
login)
    do_mirror=false
    do_backup=false
    leave=false
    ;;
pull)
    do_mirror=true
    do_backup=true
    ;;
push)
    do_mirror=true
    do_backup=false
    mirror_opts+=" --reverse "
    ;;
*)
    echo Usage: $0 "<pull|push|login> [--dry-run] [--stay] [--dont-backup]"
    exit
    ;;
esac

for arg in $@; do
    case $arg in
    --dry-run)
        mirror_opts+=" --dry-run "
        ;;
    --dont-backup)
        do_backup=false
        ;;
    --stay)
        leave=false
        ;;
    esac
done

if $do_backup; then
    echo "First, ensuring local assets are backed up..."
    ./backup_assets.sh
    echo "... Done."
fi

script="\
    set ssl:verify-certificate no; \
    set ftp:list-options -a; \
    set xfer:log-file lftp_assets.log; \
    set xfer:log true; \
    set xfer:backup-suffix bak; \
    set xfer:make-backup true; \
    set xfer:keep-backup true; \
"

mirror_opts+=" --only-newer --verbose=3 \
    --exclude-glob=.git* --exclude-glob=*.bak "

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
