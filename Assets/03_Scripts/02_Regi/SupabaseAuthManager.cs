using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.Text;
using System;

public class SupabaseAuthManager : MonoBehaviour
{
    // UI references for input fields and feedback text
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField nameInput;
    public TMP_Text feedbackText;

    // Supabase API config (replace with your actual project values)
    private string supabaseUrl = "https://your-project-id.supabase.co";
    private string supabaseApiKey = "your-public-anon-key";
    private string accessToken = "";

    void Awake()
    {
        // Keep this object alive across scene loads
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Check if there's a saved session token and restore it
        if (PlayerPrefs.HasKey("supabase_access_token"))
        {
            accessToken = PlayerPrefs.GetString("supabase_access_token");
            StartCoroutine(FetchUserProfile());
            feedbackText.text = "Session restored.";
        }
        else
        {
            feedbackText.text = "No active session.";
        }
    }

    // Called by UI button to log in the user
    public void Login()
    {
        StartCoroutine(LoginRoutine(emailInput.text, passwordInput.text));
    }

    // Called by UI button to register a new user
    public void Register()
    {
        StartCoroutine(RegisterRoutine(emailInput.text, passwordInput.text));
    }

    // Called by UI button to initiate password recovery
    public void ForgotPassword()
    {
        StartCoroutine(ForgotPasswordRoutine(emailInput.text));
    }

    // Called by UI button to update user profile name
    public void UpdateProfile()
    {
        StartCoroutine(UpdateUserProfileRoutine(nameInput.text));
    }

    // Logs the user out and clears session token
    public void Logout()
    {
        accessToken = "";
        PlayerPrefs.DeleteKey("supabase_access_token");
        feedbackText.text = "Logged out.";
    }

    // Coroutine to log in the user using email and password
    IEnumerator LoginRoutine(string email, string password)
    {
        string url = $"{supabaseUrl}/auth/v1/token?grant_type=password";

        var body = new { email, password };
        string jsonBody = JsonUtility.ToJson(body);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("apikey", supabaseApiKey);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            var tokenResponse = JsonUtility.FromJson<TokenResponse>(json);
            accessToken = tokenResponse.access_token;
            PlayerPrefs.SetString("supabase_access_token", accessToken);
            feedbackText.text = "Login successful.";
            StartCoroutine(FetchUserProfile());
        }
        else
        {
            feedbackText.text = "Login failed: " + request.error;
        }
    }

    // Coroutine to register a new user
    IEnumerator RegisterRoutine(string email, string password)
    {
        string url = $"{supabaseUrl}/auth/v1/signup";

        var body = new { email, password };
        string jsonBody = JsonUtility.ToJson(body);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("apikey", supabaseApiKey);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            feedbackText.text = "Registration successful. Please check your email to confirm.";
        }
        else
        {
            feedbackText.text = "Registration failed: " + request.error;
        }
    }

    // Coroutine to trigger password recovery email
    IEnumerator ForgotPasswordRoutine(string email)
    {
        string url = $"{supabaseUrl}/auth/v1/recover";

        var body = new { email };
        string jsonBody = JsonUtility.ToJson(body);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("apikey", supabaseApiKey);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            feedbackText.text = "Recovery email sent. Check your inbox.";
        }
        else
        {
            feedbackText.text = "Failed to send recovery email: " + request.error;
        }
    }

    // Coroutine to update the user's profile information
    IEnumerator UpdateUserProfileRoutine(string fullName)
    {
        string url = $"{supabaseUrl}/auth/v1/user";

        var body = new { data = new { full_name = fullName } };
        string jsonBody = JsonUtility.ToJson(body);

        UnityWebRequest request = new UnityWebRequest(url, "PUT");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + accessToken);
        request.SetRequestHeader("apikey", supabaseApiKey);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            feedbackText.text = "Profile updated successfully.";
        }
        else
        {
            feedbackText.text = "Profile update failed: " + request.error;
        }
    }

    // Coroutine to fetch the currently logged-in user's profile
    IEnumerator FetchUserProfile()
    {
        string url = $"{supabaseUrl}/auth/v1/user";

        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Authorization", "Bearer " + accessToken);
        request.SetRequestHeader("apikey", supabaseApiKey);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            feedbackText.text = "User profile loaded.";
        }
        else
        {
            feedbackText.text = "Failed to fetch user profile.";
        }
    }

    // Class for token response mapping from Supabase login endpoint
    [Serializable]
    public class TokenResponse
    {
        public string access_token;
        public string token_type;
        public string expires_in;
        public string refresh_token;
        public string user;
    }
}

