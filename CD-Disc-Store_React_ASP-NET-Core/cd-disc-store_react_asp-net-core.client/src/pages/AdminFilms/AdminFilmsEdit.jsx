import React, { useState, useEffect } from 'react'
import { useParams } from 'react-router-dom';
import AdminPanelHeader from '../../AdminPanelHeader/AdminPanelHeader';
import './../AdminDisks/admindisksadd.css';


const AdminFilmsEdit = () => {
    let { id } = useParams();
    const [name, setName] = useState("");
    const [genre, setGenre] = useState("");
    const [producer, setProducer] = useState("");
    const [mainRole, setMainRole] = useState("");
    const [ageLimit, setAgeLimit] = useState(0);
    const [coverImagePath, setCoverImagePath] = useState("");
    const [imageStorageName, setImageStorageName] = useState("");
    const [message, setMessage] = useState("");

    function validateForm() {
        return name.length > 0 && genre.length > 0 && producer.length > 0 && mainRole.length > 0 && ageLimit >= 0 && ageLimit <= 30 && coverImagePath.length > 0 && imageStorageName.length > 0;
    }

    let handleSubmit = async (e) => {
        e.preventDefault();
        try {
          if (!validateForm()) {setMessage("Please fill all the fields with valid values"); return;}
          let identifier = id.split("=")[1];
          //console.log(identifier);
          let url="https://localhost:7117/Film/Edit?"+"existingFilmId="+identifier+"&"+id+"&Name="+name+"&Genre="+genre+"&Producer="+producer+"&MainRole="+mainRole+"&AgeLimit="+ageLimit+"&CoverImagePath="+coverImagePath+"&ImageStorageName="+imageStorageName;
          //console.log(url);
          let res = await fetch(url, {
            method: "PUT"
          });
          let resJson = await res.json();
          if (res.status === 200) {
            
            setMessage("Film edited successfully");
          } else {
            setMessage("Some error occured");
          }
        } catch (err) {
          console.log(err);
        }
      };
      useEffect(() => {
        fetch(`https://localhost:7117/Film/GetFilm?${id}`).then(res => res.json()).then(data => {
            setName(data.name);
            setGenre(data.genre);
            setProducer(data.producer);
            setMainRole(data.mainRole);
            setAgeLimit(data.ageLimit);
            setCoverImagePath(data.coverImagePath);
            setImageStorageName(data.imageStorageName);
        }); 
      },[id])
    
    
    return (
        <>
            <AdminPanelHeader />
            <div className='adminadd'>
                <h1>Edit film</h1>
                <form onSubmit={handleSubmit}>
                    <input type="text" name="name" id="name" placeholder='Name' value={name} onChange={(e) => setName(e.target.value)}/>
                    <input type="text" name="genre" id="genre" placeholder='Genre' value={genre} onChange={(e) => setGenre(e.target.value)}/>
                    <input type="text" name="producer" id="producer" placeholder='Producer' value={producer} onChange={(e) => setProducer(e.target.value)}/>
                    <input type="text" name="mainRole" id="mainRole" placeholder='Main role' value={mainRole} onChange={(e) => setMainRole(e.target.value)}/>
                    <input type="number" step="1" min="0" name="ageLimit" id="ageLimit" placeholder='Age limit' value={ageLimit} onChange={(e) => setAgeLimit(e.target.value)}/>
                    <input type="text" name="coverImagePath" id="coverImagePath" placeholder='Cover image path' value={coverImagePath} onChange={(e) => setCoverImagePath(e.target.value)}/>
                    <input type="text" name="imageStorageName" id="imageStorageName" placeholder='Image storage name' value={imageStorageName} onChange={(e) => setImageStorageName(e.target.value)}/>

                    <button type="submit">Edit</button>

                    <div className="message">{message ? <p>{message}</p> : null}</div>
                </form>
                
            </div>
        </>
    )
}

export default AdminFilmsEdit
