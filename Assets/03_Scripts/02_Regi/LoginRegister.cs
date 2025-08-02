using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Net.Mail;
using Firebase;
using Firebase.Auth;
using System;
using System.Threading.Tasks;
using Firebase.Extensions;

public class LoginRegister : MonoBehaviour
{
    public GameObject loginPanel, signupPanel, profilePanel, forgotPassPanel, notifPanel, posNotifPanel, homePanel, navbar, confirmationPanel, privacyPolicyPanel, settingsPanel, editProfPanel;
    public TMP_InputField loginEmail, loginUsername, loginPass, signupName, signupEmail, signupUsername, signupPass, signupCPass, forgetPassEmail;
    public TMP_Dropdown signupSection;
    public TMP_Text notif_Title, notif_Message;
    public TMP_Text headUname, profileName, profileEmail, profileUsername, profileSection;

    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;

    bool isSignIn = false;

    private void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.

                InitializeFirebase();

                void InitializeFirebase()
                {

                    auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                    auth.StateChanged += AuthStateChanged;
                    AuthStateChanged(this, null);

                    Debug.Log("Firebase Auth Initialized");
                }
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    public void ShowLoginPanel()
    {
        loginPanel.SetActive(true);
        signupPanel.SetActive(false);
        profilePanel.SetActive(false);
        forgotPassPanel.SetActive(false);
    }

    public void ShowSignupPanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(true);
        profilePanel.SetActive(false);
        forgotPassPanel.SetActive(false);

    }
    public void ShowProfilePanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        profilePanel.SetActive(true);
        forgotPassPanel.SetActive(false);
    }
    public void HideProfilePanel()
    {
        profilePanel.SetActive(false);
    }
    public void ShowForgetPassPanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        profilePanel.SetActive(false);
        forgotPassPanel.SetActive(true);
    }

    public void ShowHomePanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        profilePanel.SetActive(false);
        forgotPassPanel.SetActive(false);
        homePanel.SetActive(true);
        navbar.SetActive(true);
    }

    public void ShowEditProfile()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        profilePanel.SetActive(false);
        editProfPanel.SetActive(true);
        forgotPassPanel.SetActive(false);
        homePanel.SetActive(false);
        navbar.SetActive(false);
    }
    public void ShowConfirmationPanel()
    {
        confirmationPanel.SetActive(true);
    }
    public void HideConfirmationPanel()
    {
        confirmationPanel.SetActive(false);
    }
    public void ShowPrivacyPolicyPanel()
    {
        privacyPolicyPanel.SetActive(true);
    }
    public void HidePrivacyPolicyPanel()
    {
        privacyPolicyPanel.SetActive(false);
    }
    public void ShowSettingsPanel()
    {
        settingsPanel.SetActive(true);
    }
    public void HideSettingsPanel()
    {
        settingsPanel.SetActive(false);
    }
    public void LoadLessonScene()
    {
        SceneManager.LoadScene("04_LESSON");
    }
    public void LoadChatbotScene()
    {
        SceneManager.LoadScene("03_SEARCH");
    }
    public void LoadLeaderboardsScene()
    {
        SceneManager.LoadScene("06_LEADERBOARD");
    }
    public void LoadFirstScene()
    {
        SceneManager.LoadScene("01_STARTING");
    }



    public void LoginUser()
    {
        if (string.IsNullOrEmpty(loginEmail.text) || string.IsNullOrEmpty(loginPass.text) || string.IsNullOrEmpty(loginUsername.text))
        {
            ShowNotification("Error", "Please fill in all fields.");
            return;
        }
        else
        {
            ShowPosNotification("Congrats", "Login successful.");
            ShowHomePanel();
        }
        SignInUser(loginEmail.text, loginPass.text);
    }

    public void SignUpUser()
    {
        if (string.IsNullOrEmpty(signupName.text) || string.IsNullOrEmpty(signupEmail.text) || string.IsNullOrEmpty(signupUsername.text) || signupSection.value == 0 || string.IsNullOrEmpty(signupPass.text) || string.IsNullOrEmpty(signupCPass.text))
        {
            ShowNotification("Error", "Please fill in all fields.");
            return;
        }
        else if (signupPass.text != signupCPass.text)
        {
            ShowNotification("Error", "Passwords do not match.");
            return;
        }
        else if (signupPass.text.Length < 6)
        {
            ShowNotification("Error", "Password must be at least 6 characters long.");
            return;
        }

        CreateUser(signupEmail.text, signupPass.text, signupUsername.text);
        ShowHomePanel();
    }

    public void ForgotPassword()
    {
        if (string.IsNullOrEmpty(forgetPassEmail.text))
        {
            ShowNotification("Error", "Please enter your email.");
            return;
        }
        // Logic to handle password reset can be added here
        forgotPasswordSubmit(forgetPassEmail.text);
    }

    Coroutine notifCoroutine;

    public void ShowPosNotification(string title, string message)
    {
        posNotifPanel.SetActive(true);
        notif_Title.text = "" + title;
        notif_Message.text = "" + message;

        if (notifCoroutine != null)
        {
            StopCoroutine(notifCoroutine);
        }

        notifCoroutine = StartCoroutine(posHideNotificationAfterDelay(3f));
    }
    private IEnumerator posHideNotificationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ClosePosNotification();
    }


    public void ShowNotification(string title, string message)
    {
        notifPanel.SetActive(true);
        notif_Title.text = "" + title;
        notif_Message.text = "" + message;

        if (notifCoroutine != null)
        {
            StopCoroutine(notifCoroutine);
        }

        notifCoroutine = StartCoroutine(HideNotificationAfterDelay(3f));
    }
    private IEnumerator HideNotificationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        CloseNotification();
    }
    public void CloseNotification()
    {
        notifPanel.SetActive(false);
        notif_Title.text = "";
        notif_Message.text = "";
    }
    public void ClosePosNotification()
    {
        posNotifPanel.SetActive(false);
        notif_Title.text = "";
        notif_Message.text = "";
    }

    public void CloseProfile()
    {
        profilePanel.SetActive(false);

    }

    public void LogOut()
    {
        auth.SignOut();
       
        headUname.text = "";
        profileName.text = "";
        profileEmail.text = "";
        profileUsername.text = "";
        profileSection.text = "";
        ShowLoginPanel();
    }

    public void Guest()
    {
        ShowHomePanel();
        string guestName = "Guest";

       
        headUname.text = "@" + guestName + "!";
        profileUsername.text = guestName;
        profileEmail.text = guestName;
        profileName.text = guestName;
        profileSection.text = "No Section";

    }

    public void BackFScene()
    {
        SceneManager.LoadScene("01_FirstScene");
    }

    void CreateUser(string email, string password, string UserName)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);

            UpdateUserProfile(UserName);
        });
    }

    public void SignInUser(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);

            headUname.text = "@" + result.User.DisplayName + "!";
            profileUsername.text = result.User.DisplayName;
            profileEmail.text = result.User.Email;
            profileName.text = result.User.DisplayName;
            profileSection.text = signupSection.options[signupSection.value].text;


        });
    }

    void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null
                && auth.CurrentUser.IsValid();
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                isSignIn = true;

            }
        }
    }

    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    void UpdateUserProfile(string UserName)
    {
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
            {
                DisplayName = UserName,
                PhotoUrl = new System.Uri("https://via.placeholder.com/150c/0%20https://placeholder.com/"),
            };
            user.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("User profile updated successfully.");

                ShowNotification("Alert", "User profile updated successfully.");
            });
        }
    }

    bool isSigned = false;
    void Update()
    {
        if (isSignIn)
        {
            if (!isSigned)
            {
                isSigned = true;
         
                headUname.text = "@" + user.DisplayName + "!";
                profileUsername.text = "" + user.DisplayName;
                profileEmail.text = "" + user.Email;
                profileName.text = "" + user.DisplayName;
                profileSection.text = "" + signupSection.options[signupSection.value].text;

                ShowLoginPanel();
            }
        }
    }

    void forgotPasswordSubmit(string forgetPasswordEmail)
    {
        auth.SendPasswordResetEmailAsync(forgetPasswordEmail).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SendPasswordResetEmailAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                    if (firebaseEx != null)
                    {
                        var errorCode = (AuthError)firebaseEx.ErrorCode;
                        ShowNotification("Error", "Error sending password reset email");
                    }

                }

            }
            ShowPosNotification("Success", "Password reset email sent successfully. Please check your inbox.");

        });
    }
}