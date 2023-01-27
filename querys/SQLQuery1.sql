/* Д.З. Дополнить блок "Статистика за день" данными следующих категорий:
 * Самый эффективный менеджер [Фамилия, Имя] (по деньгам)
 * Самый эффективный отдел [Название] (по кол-ву проданных товаров)
 * Самый популярный товар [Название] (по кол-ву чеков)
 */
 
/*

 SELECT 
    MAX(Managers.Secname) AS Secname
    ,MAX(Managers.Name) AS Name
    ,MAX(Managers.Surname) AS Surname
FROM Sales
    LEFT JOIN Managers ON Managers.Id = Sales.ID_manager
    LEFT JOIN Products ON Products.Id = Sales.ID_product
GROUP BY Managers.Id
ORDER BY SUM(Sales.Cnt * Products.Price) DESC


*/

 SELECT 
    MAX(Managers.Secname) AS Secname
    ,MAX(Managers.Name) AS Name
    ,MAX(Managers.Surname) AS Surname
FROM Sales
    LEFT JOIN Managers ON Managers.Id = Sales.ID_manager
    LEFT JOIN Departments ON Departments.Id = Managers.
GROUP BY Managers.Id
-- ORDER BY SUM(Sales.Cnt) DESC
