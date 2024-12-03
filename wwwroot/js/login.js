const uri = "https://localhost:5224/Login";
console.log("in login");
const dom = {
    name: document.getElementById("name"),
    password: document.getElementById("password"),
    submitBtn: document.getElementById("submit")
}

dom.submitBtn.onclick = (event) => {
    event.preventDefault();

    const item = { Username: dom.name.value, password: dom.password.value }


    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },

        body: JSON.stringify(item)
    })
        .then((response) =>
            response.json()
        )
        .then((res) => {
            if (res.status == 401)
                alert("The username or password you entered is incorrect")
            else {
                if (dom.name.value === "eti" && dom.password.value === "1234")
                    localStorage.setItem("link", true);
                else
                    localStorage.setItem("link", false);

                localStorage.setItem("token", res.token);
                localStorage.setItem('userId',res.id);
                location.href = "../index.html";
            }
        })
        .catch(error => console.error('Unable to add item.', error));
}

function handleCredentialResponse(response) {
    if (response.credential) {
        var idToken = response.credential;
        var decodedToken = parseJwt(idToken);
        var userName = decodedToken.name;
        var userPassword = decodedToken.sub;
        login(userName, userPassword);
    } else {
        alert('Google Sign-In was cancelled.');
    }
}


//Parses JWT token from Google Sign-In
function parseJwt(token) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
}
// localStorage.setItem;