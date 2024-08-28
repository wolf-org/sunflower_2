using System;
using System.Text;

#if UNITY_IOS && VIRTUESKY_APPLE_AUTH
using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Extensions;
using AppleAuth.Interfaces;
using AppleAuth.Native;
#endif
using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.GameService
{
    [EditorIcon("icon_authentication")]
    public class AppleAuthentication : ServiceAuthentication
    {
        private static event Func<string> GetAuthorCodeEvent;
        private static event Func<string> GetUserIdEvent;

        private string _authorizationCode;
        private string _userId;


        private string InternalGetAuthorCode() => _authorizationCode;
        private string InternalGetUserId() => _userId;

        protected override void OnEnable()
        {
            base.OnEnable();
            GetAuthorCodeEvent += InternalGetAuthorCode;
            GetUserIdEvent += InternalGetUserId;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GetAuthorCodeEvent -= InternalGetAuthorCode;
            GetUserIdEvent -= InternalGetUserId;
        }

#if UNITY_IOS && VIRTUESKY_APPLE_AUTH
        private IAppleAuthManager _iAppleAuthManager;
#endif


        protected override void InternalInit()
        {
#if UNITY_IOS && VIRTUESKY_APPLE_AUTH
            if (AppleAuthManager.IsCurrentPlatformSupported)
            {
                // Creates a default JSON deserializer, to transform JSON Native responses to C# instances
                var deserializer = new PayloadDeserializer();
                // Creates an Apple Authentication manager with the deserializer
                this._iAppleAuthManager = new AppleAuthManager(deserializer);
            }
#endif
        }

        private void Update()
        {
#if UNITY_IOS && VIRTUESKY_APPLE_AUTH
            // Updates the AppleAuthManager instance to execute
            // pending callbacks inside Unity's execution loop
            if (this._iAppleAuthManager != null)
            {
                this._iAppleAuthManager.Update();
            }
#endif
        }

        protected override void InternalLogin()
        {
#if UNITY_IOS && VIRTUESKY_APPLE_AUTH
            var loginArgs =
                new AppleAuthLoginArgs(LoginOptions.IncludeEmail | LoginOptions.IncludeFullName);

            this._iAppleAuthManager.LoginWithAppleId(
                loginArgs,
                credential =>
                {
                    // Obtained credential, cast it to IAppleIDCredential
                    if (credential is IAppleIDCredential appleIdCredential)
                    {
                        // Apple User ID
                        // You should save the user ID somewhere in the device
                        var userId = appleIdCredential.User;

                        // Email (Received ONLY in the first login)
                        var email = appleIdCredential.Email;

                        // Full name (Received ONLY in the first login)
                        var fullName = appleIdCredential.FullName;

                        // Identity token
                        var identityToken = Encoding.UTF8.GetString(
                            appleIdCredential.IdentityToken,
                            0,
                            appleIdCredential.IdentityToken.Length);

                        // Authorization code
                        var authorizationCode = Encoding.UTF8.GetString(
                            appleIdCredential.AuthorizationCode,
                            0,
                            appleIdCredential.AuthorizationCode.Length);

                        // And now you have all the information to create/login a user in your system
                        serverCode = identityToken;
                        _authorizationCode = authorizationCode;
                        _userId = userId;
                        nameAuth = $"{fullName.GivenName} {fullName.FamilyName}";
                        statusLogin = StatusLogin.Successful;
                    }
                    else
                    {
                        serverCode = "";
                        _userId = "";
                        statusLogin = StatusLogin.Failed;
                    }
                },
                error =>
                {
                    // Something went wrong
                    var authorizationErrorCode = error.GetAuthorizationErrorCode();
                    serverCode = "";
                    _userId = "";
                    statusLogin = StatusLogin.Failed;
                });
#endif
        }

        #region Api

        public static string GetAuthorizationCode() => GetAuthorCodeEvent?.Invoke();
        public static string GetUserId() => GetUserIdEvent?.Invoke();

        #endregion
    }
}