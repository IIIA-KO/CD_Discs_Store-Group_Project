import React, { useState } from 'react'
import AdminPanelHeader from '../../AdminPanelHeader/AdminPanelHeader';

const AdminFilmsAdd = () => {
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
          let res = await fetch("https://localhost:7117/Film/Create?Name="+name+"&Genre="+genre+"&Producer="+producer+"&MainRole="+mainRole+"&AgeLimit="+ageLimit+"&CoverImagePath="+coverImagePath+"&ImageStorageName="+imageStorageName, {
            method: "POST"
          });
          let resJson = await res.json();
          if (res.status === 200) {
            
            setMessage("Film created successfully with id=" + resJson.id);
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
                <h1>Create disk</h1>
                <form onSubmit={handleSubmit}>
                    <input type="text" name="name" id="name" placeholder='Name' onChange={(e) => setName(e.target.value)}/>
                    <input type="text" name="genre" id="genre" placeholder='Genre' onChange={(e) => setGenre(e.target.value)}/>
                    <input type="text" name="producer" id="producer" placeholder='Producer' onChange={(e) => setProducer(e.target.value)}/>
                    <input type="text" name="mainRole" id="mainRole" placeholder='Main role' onChange={(e) => setMainRole(e.target.value)}/>
                    <input type="number" step="1" min="0" name="ageLimit" id="ageLimit" placeholder='Age limit' onChange={(e) => setAgeLimit(e.target.value)}/>
                    <input type="text" name="coverImagePath" id="coverImagePath" placeholder='Cover image path' onChange={(e) => setCoverImagePath(e.target.value)}/>
                    <input type="text" name="imageStorageName" id="imageStorageName" placeholder='Image storage name' onChange={(e) => setImageStorageName(e.target.value)}/>

                    <button type="submit">Create</button>

                    <div className="message">{message ? <p>{message}</p> : null}</div>
                </form>
                
            </div>
        </>
    )
}

export default AdminFilmsAdd
