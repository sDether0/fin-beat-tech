**Для запуска необходима поднятая субд Postgres, и строка подключения в env переменной ConnectionString**

*Опционально можно указать env переменную* **APP_URL** *для указания адреса прослушивания, по умолчанию будет http://0.0.0.0:8080*

P.S. ТЗ противоречит best practice rest api по структуре json, было бы лучше привести входные данные к виду

```json
{
    "code":"1",
    "value":"value1"
}
```
Но в проекте реализовано по ТЗ.


2 задача

Наименование клиентов и кол-во контактов клиентов
```sql
SELECT 
    c.ClientName,
    COUNT(cc.Id) AS ContactCount
    FROM Clients c
    LEFT JOIN ClientContacts cc ON cc.ClientId = c.Id
    GROUP BY c.ClientName;
```

Список клиентов, у которых есть более 2 контактов
```sql
SELECT 
    c.ClientName,
    COUNT(cc.Id) AS ContactCount
FROM Clients c
JOIN ClientContacts cc ON cc.ClientId = c.Id
GROUP BY c.Id, c.ClientName
HAVING COUNT(cc.Id) > 2;
```

3 задача

```sql
WITH OrderedDates AS (
    SELECT 
        Id,
        Dt AS Sd,
        LEAD(Dt) OVER (PARTITION BY Id ORDER BY Dt) AS Ed
    FROM Dates
)
SELECT 
    Id, 
    Sd, 
    Ed
FROM OrderedDates
WHERE Ed IS NOT NULL;

```
