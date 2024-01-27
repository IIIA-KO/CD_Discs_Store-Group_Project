import { useEffect, useRef } from 'react'
import React from 'react'

import '../Slider/Slider.css'
import Card from '../pages/Card/Card';


export default function TopFilms({ data }) {


    const postTopFilm = data.map((item) => {
        const { id } = item
        return (<div className='cardo'><Card item={item} key={id} /></div>);
    });

    return (
        <div className="conteudo">
            <div className='carrousel'>
                {postTopFilm}
            </div>
        </div>
    );
}
