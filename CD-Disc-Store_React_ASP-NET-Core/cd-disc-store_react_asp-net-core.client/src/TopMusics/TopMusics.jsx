import React from 'react'
import '../Slider/Slider.css'
import CardMusic from '../pages/CardMusic/CardMusic';
export default function TopMusics({data}) {
    const postTopMusic = data.map((item) => {
        const { id } = item
        return (<div className='cardo'><CardMusic item={item} key={id} /></div>);
    });

    return <div className="conteudo"><div className='carrousel'>{postTopMusic}</div></div>;
}
