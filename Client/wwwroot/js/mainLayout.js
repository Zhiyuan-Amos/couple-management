// Referenced from https://docs.microsoft.com/en-us/azure/active-directory/develop/scenario-spa-acquire-token?tabs=javascript2#acquire-a-token-with-a-redirect
// and https://docs.microsoft.com/en-us/azure/active-directory/develop/msal-js-initializing-client-applications
// and https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-browser/docs/working-with-b2c.md#b2c-app-configuration
async function login(clientId, authority, knownAuthority, redirectUri) {
    const msalConfig = {
        auth: {
            clientId: clientId,
            authority: authority,
            knownAuthorities: [knownAuthority],
            redirectUri: redirectUri,
        },
        cache: {
            cacheLocation: "localStorage",
            storeAuthStateInCookie: false,
        },
    };

    const publicClientApplication = new msal.PublicClientApplication(msalConfig);

    const redirectResponse = await publicClientApplication.handleRedirectPromise();
    if (redirectResponse === null) {
        const account = publicClientApplication.getAllAccounts()[0];
        const accessTokenRequest = {
            scopes: ["openid", "offline_access", "https://couplesg.onmicrosoft.com/api/all"],
            account: account
        };

        try {
            await publicClientApplication.acquireTokenSilent(accessTokenRequest);
        } catch (error) {
            console.error(error);
            if (error.name === "BrowserAuthError") {
                await publicClientApplication.acquireTokenRedirect(accessTokenRequest);
            }
        }
    }
}
