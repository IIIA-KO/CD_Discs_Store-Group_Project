import React, { useState } from 'react'
import AdminPanelHeader from '../../AdminPanelHeader/AdminPanelHeader';

const AdminMusicAdd = () => {
    const [name, setName] = useState("");
    const [genre, setGenre] = useState("");
    const [artist, setArtist] = useState("");
    const [language, setLanguage] = useState("");
    const [coverImagePath, setCoverImagePath] = useState("");
    const [imageStorageName, setImageStorageName] = useState("");
    const [message, setMessage] = useState("");

    function validateForm() {
        return name.length > 0 && genre.length > 0 && artist.length > 0 && language.length > 0 && coverImagePath.length > 0 && imageStorageName.length > 0;
    }

    let handleSubmit = async (e) => {
        e.preventDefault();
        try {
          if (!validateForm()) {setMessage("Please fill all the fields with valid values"); return;}
          let res = await fetch("https://localhost:7117/Music/Create?Name="+name+"&Genre="+genre+"&Artist="+artist+"&Language="+language+"&CoverImagePath="+coverImagePath+"&ImageStorageName="+imageStorageName, {
            method: "POST"
          });
          let resJson = await res.json();
          if (res.status === 200) {
            
            setMessage("Music created successfully");
          } else {
            setMessage("Some error occured");
          }
        } catch (err) {
          console.log(err);
        }
      };
    
    return (
        <>
            <AdminPanelHeader />
            <div className='admindisks'>
                <h1>Create music</h1>
                <form onSubmit={handleSubmit}>
                    <input type="text" name="name" id="name" placeholder='Name' onChange={(e) => setName(e.target.value)}/>
                    <input type="text" name="genre" id="genre" placeholder='Genre' onChange={(e) => setGenre(e.target.value)}/>
                    <input type="text" name="artist" id="artist" placeholder='Artist' onChange={(e) => setArtist(e.target.value)}/>
                    <input type="text" name="language" id="language" placeholder='Language' onChange={(e) => setLanguage(e.target.value)}/>
                    <input type="text" name="coverImagePath" id="coverImagePath" placeholder='Cover image path' onChange={(e) => setCoverImagePath(e.target.value)}/>
                    <input type="text" name="imageStorageName" id="imageStorageName" placeholder='Image storage name' onChange={(e) => setImageStorageName(e.target.value)}/>

                    <button type="submit">Create</button>

                    <div className="message">{message ? <p>{message}</p> : null}</div>
                </form>
                
            </div>
        </>
    )
}

export default AdminMusicAdd
