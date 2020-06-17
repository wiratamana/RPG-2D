using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

public class UserSearchForm : MonoBehaviour
{
    [SerializeField] private TMP_InputField searchInput;
    [SerializeField] private UserInfo prefab;
    [SerializeField] private Transform userInfoParent;
    [SerializeField] private UITweenFormTransition toHomeTransition;
    private List<UserInfo> userInfos = new List<UserInfo>();

    private float waitingTime;
    private bool isSearchRunning;

    private enum SearchResult
    {
        Success = 0,
        NotFound = 2,
        Error = 1,
    }

    public void OnSearchTextChanged(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            waitingTime = -1.0f;
            isSearchRunning = false;
        }
        else
        {
            waitingTime = 0.5f;

            if (isSearchRunning == false)
            {
                isSearchRunning = true;
                StartCoroutine(Search());
            }
        }
    }

    public void OnClickBackToHome()
    {
        void resetForm()
        {
            searchInput.text = null;
            DestroyAllUserInfos();
            toHomeTransition.OnTransitionCompleted -= resetForm;
        }

        toHomeTransition.OnTransitionCompleted += resetForm;
        toHomeTransition.Execute();
    }

    private IEnumerator Search()
    {
        while (isSearchRunning == true && waitingTime > 0.0f)
        {
            if (userInfos.Count > 0)
            {
                for (int i = 0; i < userInfos.Count; i++)
                {
                    userInfos[i].Destroy();
                }

                userInfos.Clear();
            }

            waitingTime = Mathf.MoveTowards(waitingTime, 0.0f, Time.deltaTime);
            yield return null;
        }

        if (waitingTime < 0)
        {
            yield break;
        }

        var getUsers = GetUsers(searchInput.text);
        while (getUsers.IsCompleted == false)
        {
            yield return null;
        }

        // TODO : Handle exception if Result is null or empty
        // TODO : Handle exception if string result was not expected

        var splittedResult = getUsers.Result.Split(new char[] { '|' }, System.StringSplitOptions.RemoveEmptyEntries);
        var searchResult = (SearchResult)System.Convert.ToInt32(splittedResult[0]);
        if (searchResult != SearchResult.Success)
        {
            Debug.Log($"Search Failed ! Reason : {searchResult}");
        }
        else
        {
            for (int i = 1; i < splittedResult.Length; i++)
            {
                var splittedUserInfo = splittedResult[i].Split(',');
                var username = splittedUserInfo[0];
                var gender = (Gender)System.Enum.Parse(typeof(Gender), splittedUserInfo[1]);
                var birthday = splittedUserInfo[2];

                var userInfo = Instantiate(prefab, userInfoParent);
                userInfo.SetValue(username, birthday, gender);
                userInfos.Add(userInfo);
            }
        }

        isSearchRunning = false;
    }

    private void DestroyAllUserInfos()
    {
        if (userInfos.Count > 0)
        {
            for (int i = 0; i < userInfos.Count; i++)
            {
                userInfos[i].Destroy();
            }

            userInfos.Clear();
        }
    }

    private async Task<string> GetUsers(string keyword)
    {
        // TODO : Handle exception from http

        using (var http = new HttpClient())
        {
            var formValue = new Dictionary<string, string>() { { "keyword", keyword } };

            var form = new FormUrlEncodedContent(formValue);

            var content = await http.PostAsync(ServerAddress.SearchUserFormAdderss, form);
            var result = await content.Content.ReadAsStringAsync();

            form.Dispose();
            return result;
        }
    }
}
