using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Data
{
    public enum CountyTypeEnum
    {
        [Display(Name = "Not A County", ShortName = "not-a-county")] NotACounty = 0,
        [Display(Name = "Alameda", ShortName = "alameda")] Alameda = 1,
        [Display(Name = "Alpine", ShortName = "alpine")] Alpine = 2,
        [Display(Name = "Amador", ShortName = "amador")] Amador = 3,
        [Display(Name = "Butte", ShortName = "butte")] Butte = 4,
        [Display(Name = "Calaveras", ShortName = "calaveras")] Calaveras = 5,
        [Display(Name = "Colusa", ShortName = "colusa")] Colusa = 6,
        [Display(Name = "Contra Costa", ShortName = "contra-costa")] ContraCosta = 7,
        [Display(Name = "Del Norte", ShortName = "del-norte")] DelNorte = 8,
        [Display(Name = "El Dorado", ShortName = "el-dorado")] ElDorado = 9,
        [Display(Name = "Fresno", ShortName = "fresno")] Fresno = 10,
        [Display(Name = "Glenn", ShortName = "glenn")] Glenn = 11,
        [Display(Name = "Humboldt", ShortName = "humboldt")] Humboldt = 12,
        [Display(Name = "Imperial", ShortName = "imperial")] Imperial = 13,
        [Display(Name = "Inyo", ShortName = "inyo")] Inyo = 14,
        [Display(Name = "Kern", ShortName = "kern")] Kern = 15,
        [Display(Name = "Kings", ShortName = "kings")] Kings = 16,
        [Display(Name = "Lake", ShortName = "lake")] Lake = 17,
        [Display(Name = "Lassen", ShortName = "lassen")] Lassen = 18,
        [Display(Name = "Los Angeles", ShortName = "los-angeles")] LosAngeles = 19,
        [Display(Name = "Madera", ShortName = "madera")] Madera = 20,
        [Display(Name = "Marin", ShortName = "marin")] Marin = 21,
        [Display(Name = "Mariposa", ShortName = "mariposa")] Mariposa = 22,
        [Display(Name = "Mendocino", ShortName = "mendocino")] Mendocino = 23,
        [Display(Name = "Merced", ShortName = "merced")] Merced = 24,
        [Display(Name = "Modoc", ShortName = "modoc")] Modoc = 25,
        [Display(Name = "Mono", ShortName = "mono")] Mono = 26,
        [Display(Name = "Monterey", ShortName = "monterey")] Monterey = 27,
        [Display(Name = "Napa", ShortName = "napa")] Napa = 28,
        [Display(Name = "Nevada", ShortName = "nevada")] Nevada = 29,
        [Display(Name = "Orange", ShortName = "orange")] Orange = 30,
        [Display(Name = "Placer", ShortName = "placer")] Placer = 31,
        [Display(Name = "Plumas", ShortName = "plumas")] Plumas = 32,
        [Display(Name = "Riverside", ShortName = "riverside")] Riverside = 33,
        [Display(Name = "Sacramento", ShortName = "sacramento")] Sacramento = 34,
        [Display(Name = "San Benito", ShortName = "san-benito")] SanBenito = 35,
        [Display(Name = "San Bernardino", ShortName = "san-bernardino")] SanBernardino = 36,
        [Display(Name = "San Diego", ShortName = "san-diego")] SanDiego = 37,
        [Display(Name = "San Francisco", ShortName = "san-francisco")] SanFrancisco = 38,
        [Display(Name = "San Joaquin", ShortName = "san-joaquin")] SanJoaquin = 39,
        [Display(Name = "San Luis Obispo", ShortName = "san-luis-obispo")] SanLuisObispo = 40,
        [Display(Name = "San Mateo", ShortName = "san-mateo")] SanMateo = 41,
        [Display(Name = "Santa Barbara", ShortName = "santa-barbara")] SantaBarbara = 42,
        [Display(Name = "Santa Clara", ShortName = "santa-clara")] SantaClara = 43,
        [Display(Name = "Santa Cruz", ShortName = "santa-cruz")] SantaCruz = 44,
        [Display(Name = "Shasta", ShortName = "shasta")] Shasta = 45,
        [Display(Name = "Sierra", ShortName = "sierra")] Sierra = 46,
        [Display(Name = "Siskiyou", ShortName = "siskiyou")] Siskiyou = 47,
        [Display(Name = "Solano", ShortName = "solano")] Solano = 48,
        [Display(Name = "Sonoma", ShortName = "sonoma")] Sonoma = 49,
        [Display(Name = "Stanislaus", ShortName = "stanislaus")] Stanislaus = 50,
        [Display(Name = "Sutter", ShortName = "sutter")] Sutter = 51,
        [Display(Name = "Tehama", ShortName = "tehama")] Tehama = 52,
        [Display(Name = "Trinity", ShortName = "trinity")] Trinity = 53,
        [Display(Name = "Tulare", ShortName = "tulare")] Tulare = 54,
        [Display(Name = "Tuolumne", ShortName = "tuolumne")] Tuolumne = 55,
        [Display(Name = "Ventura", ShortName = "ventura")] Ventura = 56,
        [Display(Name = "Yolo", ShortName = "yolo")] Yolo = 57,
        [Display(Name = "Yuba", ShortName = "yuba")] Yuba = 58,
        [Display(Name = "Statewide", ShortName = "statewide")] Statewide = 59
    }

}
