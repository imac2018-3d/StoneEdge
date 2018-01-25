folder_id=1ByU_jMm3NDlYJfOJXAb3LZZuuYq186Dy

case $1 in
    pull)
        gdrive sync download $folder_id Art
        ;;
    push)
        gdrive sync upload Art $folder_id
        ;;
    *)
        echo "Usage: $0 <pull|push>"
        exit
        ;;
esac

