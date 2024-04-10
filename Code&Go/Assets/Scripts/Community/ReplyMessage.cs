using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

[System.Serializable]
public enum MessageReplyID {
    SuccessfulLogin,
    FailedLogin,
    SuccessfulRegistration,
    FailedRegistration,
    ShortPlaylistNameError,
    LongPlaylistNameError,
    SuccessfulCreatedPlaylist,
    ErrorCreatingPlaylist,
    FailedUsernameEmpty,
    FailedPasswordEmpty,
    LevelUploadedCorrectly,
    ErrorUploadingLevel,
    ErrorPlaylistEmpty,
    AddedClassSuccessfully,
    ClassNotFound,
    ServerNotFound,
    LevelsNotFound,
    PlaylistNotFound,
    NotBelongToAnyClass,
    PasswordDoesNotMatch,
    IllegalCharacters
}

[System.Serializable]
public struct MessageReply {
    public MessageReplyID messageReplyID;
    public LocalizedString localizeString;
    public bool isError;
}

public class ReplyMessage : MonoBehaviour {

    [SerializeField] private Text text;
    [SerializeField] private LocalizeStringEvent localizeStringEvent;
    [SerializeField] private List<MessageReply> MessageReplys;
    [SerializeField] private Image bgImage;
    [SerializeField] private Image buttonImage;

    [SerializeField] private Color errorColor1;
    [SerializeField] private Color errorColor2;
    [SerializeField] private Color infoColor1;
    [SerializeField] private Color infoColor2;

    public void Configure(MessageReplyID id) {
        foreach(MessageReply mr in MessageReplys) {
            if(mr.messageReplyID == id) {
                localizeStringEvent.StringReference = mr.localizeString;
                localizeStringEvent.RefreshString();
                text.text = localizeStringEvent.StringReference.GetLocalizedStringAsync().Result;
                bgImage.color = mr.isError ? errorColor1 : infoColor1;
                buttonImage.color = mr.isError ? errorColor2 : infoColor2;
            }
        }
        gameObject.SetActive(true);
    }
}