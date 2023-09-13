using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.Stuff
{
    public static class Retry
    {
        public static async Task<bool> Do(Func<bool> action, Func<bool, bool> callback, TimeSpan interval, int maxAttemptCount, string entity)
        {
            var result = false;

            await Task.Run(async () =>
            {

                for (int i = 1; i <= maxAttemptCount; i++)
                {
                    try
                    {
                        result = action.Invoke();
                        var retry = callback.Invoke(result);
                        if (retry == true)
                        {
                            await Task.Delay(interval);
                            Logger.Instance.LogError($"Подключение на удалось ({entity}). Попытка {i}/{maxAttemptCount}");
                            continue;
                        }

                        return;
                    }
                    catch (Exception e)
                    {
                        Logger.Instance.LogError($"Подключение на удалось ({entity}). Попытка {i}/{maxAttemptCount}", e);
                        await Task.Delay(interval);
                    }
                }

            });

            return result;
        }
    }
}
