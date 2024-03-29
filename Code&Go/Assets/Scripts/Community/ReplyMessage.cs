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
    ErrorUploadingLevel
}

[System.Serializable]
public struct MessageReply {
    public MessageReplyID messageReplyID;
    public LocalizedString localizeString;
}

public class ReplyMessage : MonoBehaviour {

    [SerializeField] private Text text;
    [SerializeField] private LocalizeStringEvent localizeStringEvent;
    [SerializeField] private List<MessageReply> MessageReplys;

    public void Configure(MessageReplyID id) {
        foreach(MessageReply mr in MessageReplys) {
            if(mr.messageReplyID == id) {
                localizeStringEvent.StringReference = mr.localizeString;
                localizeStringEvent.RefreshString();
                text.text = localizeStringEvent.StringReference.GetLocalizedStringAsync().Result;
            }
        }
        gameObject.SetActive(true);
    }
}