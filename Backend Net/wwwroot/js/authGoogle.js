// wwwroot/js/site.js

import { initializeApp } from "https://www.gstatic.com/firebasejs/10.12.2/firebase-app.js";
import { getAuth, signInWithPopup, GoogleAuthProvider } from "https://www.gstatic.com/firebasejs/10.12.2/firebase-auth.js";

// Configuración de Firebase
const firebaseConfig = {

    apiKey: "AIzaSyDmiAT-6KqKyNCX6b-T6y7TQwV5gU0jjO0",

    authDomain: "backendnet-c442c.firebaseapp.com",

    projectId: "backendnet-c442c",

    storageBucket: "backendnet-c442c.appspot.com",

    messagingSenderId: "296296323514",

    appId: "1:296296323514:web:596bf5a9dda9b9a5f76583",

    measurementId: "G-RTWE5NG2YZ"

};


// Inicializar Firebase
const app = initializeApp(firebaseConfig);
const auth = getAuth(app);
const provider = new GoogleAuthProvider(auth);

// Función para iniciar sesión con Google
window.signInWithGoogle = function () {
    signInWithPopup(auth, provider)
        .then((result) => {
            const credential = GoogleAuthProvider.credentialFromResult(result);
            const token = credential.accessToken;
            const user = result.user;
            console.log("Usuario autenticado:", user);
        })
        .catch((error) => {
            console.error("Error al iniciar sesión con Google:", error.message);
        });
};
