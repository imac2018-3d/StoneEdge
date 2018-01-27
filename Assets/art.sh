folder_id=1ByU_jMm3NDlYJfOJXAb3LZZuuYq186Dy
gdrive_args=${@:2}

case $1 in
    help)
        gdrive help sync download $gdrive_args
        ;;
    pull)
        gdrive sync download $folder_id Art $gdrive_args
        ;;
    push)
        gdrive sync upload Art $folder_id $gdrive_args
        ;;
    *)
        echo "Usage: $0 <pull|push>"
        exit
        ;;
esac

