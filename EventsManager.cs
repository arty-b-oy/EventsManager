using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
public class EventsManager : MonoBehaviour
{

    private const float cooldownBeforeSend = 5; //Ограничитель на время отправки
    private float timeoftheCastCall = 0; //Ограничитель на время отправки
    private List<string> myJson; // Список запросов
    private const string serverURL= "http://www.MyServer.ru/myJson";// URL ссылка
    private const string filePath = " Resources/JsonBuffer.txt";
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        ReadJson();
    }

    private void Update()
    {
        if (myJson.Count == 0 || Time.time - timeoftheCastCall < cooldownBeforeSend) return;
        timeoftheCastCall = Time.time;
        StartCoroutine(Upload());
    }

    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        
        for(int i = 0; i < myJson.Count; i++)
        {
            form.AddField(myJson[i], "events");
        }
        bool flag = false;
        while (!flag)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(serverURL, form))
            {
                yield return www.SendWebRequest();

                if (www.responseCode == 200)
                {
                    myJson.Clear();
                    CleanJson();
                    flag = true;
                }
            }

        }
    }



    /// <summary>
    /// Считывает все запросы из памяти
    /// </summary>
    private void ReadJson()
    {
        using (StreamReader failForBufferw = new StreamReader(filePath))
        {
            string line;
            while ((line = failForBufferw.ReadLine()) != null)
            {
                myJson.Add(line);
            }
        }
    }
    /// <summary>
    /// Добовляет новую запись запроса на сервер
    /// </summary>
    /// <param name="newJson"></param>
    private void WhriteJson(string newJson)
    {
        using (StreamWriter failForBufferw = new StreamWriter(filePath))
        {
            failForBufferw.WriteLine(newJson);
        }

    }

    /// <summary>
    /// Очистка записаных запросов в файле
    /// </summary>
    private void CleanJson()
    {
        File.WriteAllText(filePath, "");
    }
    /// <summary>
    /// Добовление события
    /// </summary>
    /// <param name="type">Тип события</param>
    /// <param name="time">Время события</param>
    /// <param name="version"> Версия клиента</param>
    protected internal void TrackEvent(string type, long time, string version)
    {
        string newJson="{'type' : '" + type +
                   "', 'time' : '" + time.ToString() +
                   "', 'version' : '" + version +
                   "'}";

        myJson.Add(newJson);
        WhriteJson(newJson);
    }
    /// <summary>
    /// Добовление события с участием номера уровня
    /// </summary>
    /// <param name="type">Тип события</param>
    /// <param name="time">Время события</param>
    /// <param name="version">Версия клиента</param>
    /// <param name="Numberlevel">Номер уровня</param>
    protected internal void TrackEvent(string type, long time, string version, int Numberlevel)
    {
        string newJson = "{'type' : '" + type +
                   "', 'time' : '" + time.ToString() +
                   "', 'version' : '" + version +
                   "', 'Numberlevel' : '" + Numberlevel.ToString() +
                   "'}";

        myJson.Add(newJson);
        WhriteJson(newJson);

    }
    /// <summary>
    /// Добовление события совершения покупки
    /// </summary>
    /// <param name="type">Тип события</param>
    /// <param name="time">Время события</param>
    /// <param name="version">Версия клиента</param>
    /// <param name="purchaseId">Идентификатор покупки</param>
    /// <param name="price">Цена покупски</param>
    protected internal void TrackEvent(string type, long time, string version, int purchaseId, int price)
    {
        string newJson = "{'type' : '" + type +
                   "', 'time' : '" + time.ToString() +
                   "', 'version' : '" + version +
                   "', 'purchaseId' : '" + purchaseId.ToString() +
                   "', 'price' : '" + price.ToString() +
                   "'}";

        myJson.Add(newJson);
        WhriteJson(newJson);

    }
}
