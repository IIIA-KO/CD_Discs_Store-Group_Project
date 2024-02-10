import React, { useState, useEffect } from 'react'
import { useParams } from 'react-router-dom';
import AdminPanelHeader from '../../AdminPanelHeader/AdminPanelHeader';

const AdminMusicEdit = () => {
    let { id } = useParams();
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
          let identifier = id.split("=")[1];
          console.log(identifier);
          let url="https://localhost:7117/Music/Edit?"+"existingMusicId="+identifier+"&"+id+"&Name="+name+"&Genre="+genre+"&Artist="+artist+"&Language="+language+"&CoverImagePath="+coverImagePath+"&ImageStorageName="+imageStorageName;
          console.log(url);
          let res = await fetch(url, {
            method: "PUT"
          });
          let resJson = await res.json();
          if (res.status === 200) {
            
            setMessage("Music edited successfully");
          } else {
            setMessage("Some error occured");
          }
        } catch (err) {
          console.log(err);
        }
      };
      useEffect(() => {
        fetch(`https://localhost:7117/Music/GetMusic?${id}`).then(res => res.json()).then(data => {
            setName(data.name);
            setGenre(data.genre);
            setArtist(data.artist);
            setLanguage(data.language);
            setCoverImagePath(data.coverImagePath);
            setImageStorageName(data.imageStorageName);
        }); 
      },[id])
    
    
    return (
        <>
            <AdminPanelHeader />
            <div className='adminadd'>
                <h1>Edit music</h1>
                <form onSubmit={handleSubmit}>
                    <input type="text" name="name" id="name" placeholder='Name' value={name} onChange={(e) => setName(e.target.value)}/>
                    <input type="text" name="genre" id="genre" placeholder='Genre' value={genre} onChange={(e) => setGenre(e.target.value)}/>
                    <input type="text" name="artist" id="artist" placeholder='Artist' value={artist} onChange={(e) => setArtist(e.target.value)}/>
                    <input type="text" name="language" id="language" placeholder='Language' value={language} onChange={(e) => setLanguage(e.target.value)}/>
                    <input type="text" name="coverImagePath" id="coverImagePath" placeholder='Cover image path' value={coverImagePath} onChange={(e) => setCoverImagePath(e.target.value)}/>
                    <input type="text" name="imageStorageName" id="imageStorageName" placeholder='Image storage name' value={imageStorageName} onChange={(e) => setImageStorageName(e.target.value)}/>

                    <button type="submit">Edit</button>

                    <div className="message">{message ? <p>{message}</p> : null}</div>
                </form>
                
            </div>
        </>
    )
}

export default AdminMusicEdit
