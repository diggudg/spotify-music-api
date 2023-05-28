## 🎵 Spotify Api

## Table of Contents

SpotifyAPI is an application that interacts with the Spotify API to provide various functionalities for users. It allows users to retrieve profile information, browse new release albums, search for albums, and like albums. The application also utilizes a SQLite database to store the refresh token for authentication with the Spotify API.

## 🚀 Features

1. **User Profile Information**

   - Users can retrieve their profile information from the Spotify API.

2. **New Release Albums**

   - Users can browse the latest release albums from their favorite artists.

3. **Search Albums**

   - Users can search for albums by title or artist name.

4. **Like Albums**

   - Users can like or favorite albums, saving them for future reference.

5. **Token Storage**
   - The application securely stores the token in a SQLite database for authentication with the Spotify API. Currently, the toke is valid for few minute only and refresh token is not configured.

## Getting Started

To run the SpotifyAPI application locally, follow these steps:

1. You need to first create or register your application in spotify developer account and get client id and client secret
1. You need to store client id and client secret in `appsettings.json` file
1. Clone the repository to your local machine
1. Open the solution in VS Code
1. Run the application
1. Application is configured with swagger, so you can test the application using swagger and also can get api documentation
