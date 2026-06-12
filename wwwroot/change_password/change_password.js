const form = document.getElementById("formToChange");

form.addEventListener("submit", async (event) => {
    event.preventDefault();
    const username = document.getElementById("username").value;
    const password = document.getElementById("password").value;
    const newPassword = document.getElementById("newPassword").value;

    try {
        await fetch("http://localhost:5169/users/changed-password", {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                Username: username,
                Password: password,
                NewPassword: newPassword
            })
        })
        .then(response => {
            if (response.ok)
            {
                window.location.href = "../home/home.html";
                return response.json();
            }
            else {
                alert(response.status);
            }
        }) 
    }
    catch (error)
    {
        alert(error);
        console.log(error);
    }
});

