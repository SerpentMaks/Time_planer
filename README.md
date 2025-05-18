graph TD
  subgraph Клиент
    A[Пользователь (любой роли)]
  end

  subgraph ASP.NET MVC
    B[Контроллеры]
    C[Модели]
    D[Представления (Views)]
  end

  subgraph SQL Server
    E[База данных]
    E1[Users]
    E2[Roles]
    E3[WorkTimeEntries]
    E4[LeaveRequests]
  end

  A --> B
  B --> C
  C --> E
  B --> D

  E --> E1
  E --> E2
  E --> E3
  E --> E4

  subgraph Роли
    R1[Сотрудник]
    R2[Регистратор]
    R3[Менеджер]
    R4[Администратор]
  end

  A --> R1
  A --> R2
  A --> R3
  A --> R4
