# Безпека

## Як повідомити про вразливість

Не відкривайте публічний issue. На вкладці [Security and quality](https://github.com/maksym-garkusha/ua-codes/security) є кнопка «Report a vulnerability»: повідомлення через неї бачить тільки власник репозиторію.

Відповідь протягом 7 днів. Винагород за знахідки немає, це навчальний проєкт без бюджету.

## Що в межах

- код бібліотеки в цьому репозиторії і workflows GitHub Actions, які збирають, підписують і викладають пакет;
- пакет `UaCodes` на nuget.org і файли в релізах на GitHub.

Сюди ж, якщо якийсь вхідний рядок змушує метод бібліотеки кинути виняток або зависнути: за задумом методи не кидають винятків на жодному вході, а на те, що не є кодом, повертають `false` або `null`. І якщо файл пакета не проходить `gh attestation verify`, як описано в [README](README.md#як-перевірити-файли).

Поза межами nuget.org, платформи GitHub і реєстри, з яких беруться коди. Неправильний результат перевірки, коли правильний код відкинуто або неправильний прийнято, це помилка, а не вразливість: про таке можна писати у звичайний issue.

## In English

Please do not open a public issue. Use the "Report a vulnerability" button on the Security and quality tab; only the repository owner sees the report. In scope: the library code, the Actions workflows that build, sign and publish it, the `UaCodes` package on nuget.org and the files attached to GitHub releases. That includes any input string that makes a method throw or hang, and any package file that fails `gh attestation verify`. Out of scope: nuget.org, GitHub's own platforms and the registers the codes come from. A wrong validation result is a bug rather than a vulnerability, so a normal issue is fine. Expect a reply within 7 days. There is no bug bounty.
