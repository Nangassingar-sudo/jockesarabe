# JokesWebApp - Application Web ASP.NET Core MVC

Bienvenue dans **JokesWebApp** ! Une application web moderne pour partager, découvrir et voter pour les meilleures blagues de la communauté.

## 📋 Architecture et Technologies

- **Framework :** ASP.NET Core MVC (.NET 10.0)
- **Base de données :** SQL Server (LocalDB)
- **ORM :** Entity Framework Core
- **Authentification :** ASP.NET Core Identity
- **UI :** Bootstrap 5
- **Langage :** C# et Razor

## 🗂️ Structure du Projet

```
JokesWebApp/
├── Controllers/
│   ├── HomeController.cs
│   └── JokesController.cs
├── Models/
│   ├── Joke.cs
│   ├── JokeQuestion.cs
│   ├── JokeAnswer.cs
│   ├── ApplicationUser.cs
│   └── ErrorViewModel.cs
├── Data/
│   ├── ApplicationDbContext.cs
│   └── Migrations/
├── Views/
│   ├── Shared/
│   │   └── _Layout.cshtml
│   ├── Home/
│   │   ├── Index.cshtml
│   │   └── Privacy.cshtml
│   └── Jokes/
│       ├── Index.cshtml
│       ├── Create.cshtml
│       ├── Edit.cshtml
│       ├── Delete.cshtml
│       └── Details.cshtml
├── Program.cs
├── appsettings.json
└── q1.csproj
```

## 🚀 Démarrage Rapide

### Prérequis
- Visual Studio 2022 ou Visual Studio Code
- .NET 10.0 SDK
- SQL Server Express ou LocalDB

### Installation et Exécution

1. **Ouvrir le projet**
   ```bash
   cd c:\Users\tchal\OneDrive\Desktop\q1
   ```

2. **Restaurer les dépendances NuGet**
   ```bash
   dotnet restore
   ```

3. **Appliquer les migrations**
   ```bash
   dotnet ef database update
   ```

4. **Lancer l'application**
   ```bash
   dotnet run
   ```

5. **Accéder à l'application**
   - L'application s'ouvre automatiquement à `https://localhost:7xxx` (le port exact dépend de votre configuration)
   - Vous pouvez accéder à l'application via `http://localhost:5xxx`

## 📚 Fonctionnalités Principales

### 🏠 Page d'Accueil
- Présentation générale de l'application
- Liens de navigation vers les blagues, création et inscription
- Guide d'utilisation rapide

### 😄 Gestion des Blagues (JokesController)

#### Actions Disponibles

| Action | Description | Authentification |
|--------|-------------|------------------|
| **Index** | Affiche toutes les blagues avec un tableau | Non requis |
| **Search** | Recherche par mot-clé dans les questions/réponses | Non requis |
| **Create** | Crée une nouvelle blague avec listes déroulantes | ✅ Requis |
| **Details** | Affiche les détails d'une blague | Non requis |
| **Edit** | Modifie une blague (créateur seulement) | ✅ Requis |
| **Delete** | Supprime une blague (créateur seulement) | ✅ Requis |
| **Like** | Ajoute un like à une blague | ✅ Requis |

### 🔐 Authentification
- Inscription (Register)
- Connexion (Login)
- Déconnexion (Logout)
- Gestion du profil (My Profile)

### 🎨 Interface Utilisateur

#### Barre de Navigation (_Layout.cshtml)
```
[Logo] | Home | Privacy | Jokes | [Search bar]
                              [Register | Login / My Profile | Logout]
```

#### Page des Blagues (Index.cshtml)
- ✅ Bouton "Créer une nouvelle blague" (bleu)
- 🔍 Barre de recherche pour filtrer les blagues
- 📊 Tableau avec colonnes :
  - Question 🤔
  - Réponse 😄
  - Likes 👍
  - Auteur
  - Actions (couleurs : Détails=bleu, Modifier=orange, Supprimer=rouge, Like=vert)

#### Formulaires (Create.cshtml, Edit.cshtml)
- **Listes déroulantes** (<select>) :
  - Question (alimentée par JokeQuestion)
  - Réponse (alimentée par JokeAnswer)
- Validation côté client et serveur
- Boutons stylisés (Submit en vert, Annuler en gris)

## 🗄️ Modèles de Données

### ApplicationUser (hérite de IdentityUser)
```csharp
public class ApplicationUser : IdentityUser
{
    public ICollection<Joke> Jokes { get; set; }
}
```

### JokeQuestion
```csharp
public class JokeQuestion
{
    public int Id { get; set; }
    public required string Text { get; set; }
    public ICollection<Joke> Jokes { get; set; }
}
```

### JokeAnswer
```csharp
public class JokeAnswer
{
    public int Id { get; set; }
    public required string Text { get; set; }
    public ICollection<Joke> Jokes { get; set; }
}
```

### Joke (Principal)
```csharp
public class Joke
{
    public int Id { get; set; }
    public int JokeQuestionId { get; set; }
    public int JokeAnswerId { get; set; }
    public required string UserId { get; set; }
    public int LikesCount { get; set; }
    
    // Relations
    public JokeQuestion? JokeQuestion { get; set; }
    public JokeAnswer? JokeAnswer { get; set; }
    public ApplicationUser? User { get; set; }
}
```

## 🔗 Relations Entre Tables

```
AspNetUsers (1) ------(∞) Jokes
JokeQuestion (1) ------(∞) Jokes
JokeAnswer (1) ------(∞) Jokes
```

## 📝 Données de Test

L'application initialise automatiquement les données suivantes au premier lancement :

### Questions de Blagues
1. "Pourquoi les plongeurs plongent-ils toujours en arrière ?"
2. "Quel est le comble pour un électricien ?"
3. "Qu'est-ce qu'un crocodile qui surveille ?"
4. "Comment appelle-t-on un chat tombé dans un pot de peinture ?"
5. "Qu'est-ce qu'un cannibale végétarien ?"

### Réponses de Blagues
1. "Parce que si ils plongeaient en avant, ils tombaient dans le bateau !"
2. "De ne pas avoir les fils à la patte !"
3. "Un Lacoste !"
4. "Un chat-peint !"
5. "Quelqu'un qui mange des haricots !"
6. "Un surveillant !"

## 🔧 Configuration

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=JokesWebAppDb;Trusted_Connection=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Sécurité dans Program.cs
- Authentification requise pour : Create, Edit, Delete, Like
- Vérification du créateur pour Edit et Delete
- Anti-Forgery Tokens sur tous les formulaires POST
- Migrations automatiques à l'application startup

## 📦 Packages NuGet Requis

```xml
<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="10.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="10.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="10.0.0" />
```

## 🛠️ Commandes Utiles

```bash
# Restaurer les dépendances
dotnet restore

# Appliquer les migrations
dotnet ef database update

# Créer une nouvelle migration
dotnet ef migrations add NomMigration

# Nettoyer les fichiers temporaires
dotnet clean

# Compiler le projet
dotnet build

# Exécuter les tests (si disponibles)
dotnet test
```

## 🎯 Flux Utilisateur Complet

1. **Visiteur** arrive sur la page d'accueil
2. **Clique** sur "Jokes" pour voir la liste
3. **S'inscrit** via le lien "Register" dans la barre de nav
4. **Crée une blague** en choisissant Question/Réponse
5. **Vote** pour les blagues préférées avec le bouton "Like"
6. **Modifie/Supprime** ses propres blagues
7. **Vérifie** ses statistiques sur "My Profile"

## 🐛 Dépannage

### La base de données n'existe pas
```bash
dotnet ef database update
```

### Erreur de connexion
Vérifiez que SQL Server/LocalDB est en cours d'exécution et que la chaîne de connexion dans `appsettings.json` est correcte.

### Migrations non appliquées
```bash
dotnet ef database update
```

### Port déjà en utilisation
Modifiez le port dans `Properties/launchSettings.json`

## 📞 Support

Pour toute question ou problème, consultez la documentation officielle :
- [Docs ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [ASP.NET Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity)

## 📄 Licence

Ce projet est fourni à titre éducatif.

---

**Version:** 1.0.0  
**Date:** 2 avril 2026  
**Auteur:** Votre Nom / Équipe
"# jockesarabe" 
"# jockesarabe" 
