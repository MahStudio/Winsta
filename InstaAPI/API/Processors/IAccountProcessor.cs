using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace InstaSharper.API.Processors
{
    public interface IAccountProcessor
    {

        Task<IResult<AccountUserResponse>> GetRequestForEditProfileAsync();
        /// <summary>
        /// NOT COMPLETE
        /// </summary>
        /// <returns></returns>
        Task<IResult<object>> SetBiographyAsync(string bio);


        Task<IResult<AccountUserResponse>> EditProfileAsync(string url, string phone, string name, string biography, string email, GenderType gender, string newUsername = null);

        Task<IResult<bool>> SetNameAndPhoneNumberAsync(string name, string phoneNumber = "");

        Task<IResult<AccountUserResponse>> RemoveProfilePictureAsync();

        Task<IResult<AccountUserResponse>> ChangeProfilePictureAsync(byte[] pictureBytes);



        // Story settings
        Task<IResult<AccountSettingsResponse>> GetStorySettingsAsync();

        Task<IResult<bool>> EnableSaveStoryToGalleryAsync();
        Task<IResult<bool>> DisableSaveStoryToGalleryAsync();

        Task<IResult<bool>> EnableSaveStoryToArchiveAsync();
        Task<IResult<bool>> DisableSaveStoryToArchiveAsync();

        Task<IResult<bool>> AllowStorySharingAsync(bool allow = true);

        Task<IResult<bool>> AllowStoryMessageRepliesAsync(MessageRepliesType repliesType);


        Task<IResult<AccountCheckResponse>> CheckUsernameAsync(string desiredUsername);


        // two factor authentication enable/disable
        /// <summary>
        /// Get security settings info & backup codes
        /// </summary>
        /// <returns></returns>
        Task<IResult<AccountSecuritySettingsResponse>> GetSecuritySettingsInfoAsync();

        Task<IResult<bool>> DisableTwoFactorAuthenticationAsync();

        Task<IResult<AccountTwoFactorSmsResponse>> SendTwoFactorEnableSmsAsync(string phoneNumber);

        Task<IResult<AccountTwoFactorResponse>> TwoFactorEnableAsync(string phoneNumber,string verificationCode);




        Task<IResult<AccountConfirmEmailResponse>> SendConfirmEmailAsync();


        Task<IResult<AccountSendSmsResponse>> SendSmsCodeAsync(string phoneNumber);
        Task<IResult<AccountVerifySmsResponse>> VerifySmsCodeAsync(string phoneNumber, string verificationCode);

        /// <summary>
        /// NOT COMPLETE dastrasi last activity
        /// </summary>
        /// <returns></returns>
        Task<IResult<object>> EnablePresenceAsync();
        /// <summary>
        /// NOT COMPLETE dastrasi last activity
        /// </summary>
        /// <returns></returns>
        Task<IResult<object>> DisablePresenceAsync();
        /// <summary>
        /// NOT COMPLETE dastrasi last activity
        /// </summary>
        /// <returns></returns>
        Task<IResult<object>> GetCommentFilterAsync();

    }
}
